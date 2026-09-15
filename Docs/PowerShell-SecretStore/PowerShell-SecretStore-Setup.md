# PowerShell SecretStore로 Codex API 키 관리하기

이 문서는 Windows PowerShell 구성 가이드다. 별도 API provider를 사용하는 Codex의 API 키를 PowerShell SecretStore에 암호화하여 저장하고, 필요한 시점에만 환경변수로 전달한다.

최종적으로 다음 명령을 제공한다.

```powershell
codex-api-main
```

`codex-api-main`은 Codex CLI에 내장된 명령이 아니다. `Microsoft.PowerShell_profile.ps1`에 사용자 함수로 정의하여, SecretStore의 API 키 주입과 Codex 실행을 한 번에 처리하는 사용자 정의 명령이다. 즉, PowerShell 프로필의 스크립트를 통해 API-key 기반 Codex 실행을 구성하기 위한 진입점이다.

| 명령 | 인증 방식 | SecretStore 사용 |
|---|---|---|
| `codex-api-main` | 별도 provider의 API 키 | 사용 |

## 1. 전체 구조

```text
codex-api-main
    → SecretStore 잠금 해제
    → API 키를 AI_API_KEY에 임시 설정
    → Codex 실행
    → 종료 후 환경변수 복원
```

API 키는 PowerShell 프로필이나 Codex `config.toml`에 기록하지 않는다. `config.toml`에는 API 키가 들어 있는 환경변수 이름만 지정한다.

## 2. 요구 사항

- Windows PowerShell 5.1 또는 PowerShell 7
- Codex CLI
- `Microsoft.PowerShell.SecretManagement` 1.1.2
- `Microsoft.PowerShell.SecretStore` 1.0.6

SecretStore 모듈은 기능 완성 상태이며 신규 기능 개발은 종료됐지만, Microsoft 안내상 보안 및 중요 버그 수정 지원은 유지된다.

## 3. SecretStore 모듈 설치

### PowerShell 7

PowerShell 7에서 실행한다.

```powershell
Install-PSResource -Name Microsoft.PowerShell.SecretManagement -Version 1.1.2 -Repository PSGallery -Scope CurrentUser -TrustRepository -AcceptLicense
Install-PSResource -Name Microsoft.PowerShell.SecretStore -Version 1.0.6 -Repository PSGallery -Scope CurrentUser -TrustRepository -AcceptLicense
```

일반적인 설치 위치:

```text
%USERPROFILE%\Documents\PowerShell\Modules\
```

### Windows PowerShell 5.1도 함께 사용하는 경우

PowerShell 7과 Windows PowerShell 5.1은 사용자 모듈 경로가 다르다. Windows PowerShell의 오래된 `PowerShellGet`에서 NuGet 초기화 오류가 발생하면 PowerShell 7에서 다음 명령을 실행한다.

```powershell
$windowsPowerShellModulePath = Join-Path ([Environment]::GetFolderPath("MyDocuments")) "WindowsPowerShell\Modules"

if (-not (Test-Path -LiteralPath $windowsPowerShellModulePath)) {
    New-Item -ItemType Directory -Path $windowsPowerShellModulePath -Force | Out-Null
}

Save-PSResource -Name Microsoft.PowerShell.SecretManagement -Version 1.1.2 -Repository PSGallery -Path $windowsPowerShellModulePath -TrustRepository -AcceptLicense
Save-PSResource -Name Microsoft.PowerShell.SecretStore -Version 1.0.6 -Repository PSGallery -Path $windowsPowerShellModulePath -TrustRepository -AcceptLicense
```

일반적인 설치 위치:

```text
%USERPROFILE%\Documents\WindowsPowerShell\Modules\
```

설치를 확인한다.

```powershell
Get-Module -ListAvailable Microsoft.PowerShell.SecretManagement, Microsoft.PowerShell.SecretStore |
    Sort-Object Name, Version -Descending |
    Select-Object Name, Version, ModuleBase
```

패키지 관리 과정에서 `Documents\PowerShell\Scripts\InstalledScriptInfos` 같은 빈 폴더가 생길 수 있다. 이는 설치된 PowerShell 스크립트의 메타데이터 경로이며 SecretStore 암호 파일이 아니다.

## 4. Codex 기본 구성

이 가이드에서는 별도의 `CODEX_HOME`을 지정하지 않고 Codex의 기본 경로를 사용한다. Windows에서는 다음 경로가 기본값 `~/.codex`에 해당한다.

```text
%USERPROFILE%\.codex
```

Codex의 provider 설정은 프로젝트 안의 `.codex/config.toml`이 아니라 사용자 기본 경로의 `config.toml`에 둔다. 공식 Codex 설정 문서상 `model_provider`와 `model_providers`는 프로젝트 로컬 설정에서 재정의할 수 없다.

파일 경로:

```text
%USERPROFILE%\.codex\config.toml
```

전체 최소 예시:

```toml
model = "PROVIDER_MODEL_ID"
model_provider = "external"

[model_providers.external]
name = "External API Provider"
base_url = "https://api.example.com"
env_key = "AI_API_KEY"
wire_api = "responses"
```

환경에 맞게 다음 값을 변경한다.

- `PROVIDER_MODEL_ID`: provider가 제공하는 모델 ID
- `external`: 원하는 provider 식별자. 단, Codex에 예약된 `openai`, `ollama`, `lmstudio`는 사용할 수 없다.
- `base_url`: 실제 provider API 주소
- `AI_API_KEY`: 아래 PowerShell 프로필에서 설정하는 환경변수 이름과 일치해야 한다.

`wire_api`는 현재 Codex 설정 계약에서 `responses`만 지원한다.

## 5. PowerShell 프로필 위치

현재 셸의 프로필 경로는 다음 명령으로 확인한다.

```powershell
$PROFILE
```

일반적인 경로:

```text
PowerShell 7
%USERPROFILE%\Documents\PowerShell\Microsoft.PowerShell_profile.ps1

Windows PowerShell 5.1
%USERPROFILE%\Documents\WindowsPowerShell\Microsoft.PowerShell_profile.ps1
```

두 환경을 모두 사용하면 아래 전체 코드를 두 프로필에 동일하게 적용한다. 다른 개인 설정이 이미 있다면 파일 전체를 덮어쓰지 말고 충돌하지 않도록 병합한다.

## 6. 전체 PowerShell 프로필 코드

코드 상단의 설정값만 환경에 맞게 변경한다.

```powershell
# Microsoft.PowerShell_profile.ps1

# ----- 사용자 설정 -----
$script:CodexProjectPath = "C:\Work\Projects\Noname\SampleProject"
$script:LocalSecretVaultName = "LocalSecretStore"
$script:ExternalProviderSecretName = "ExternalProviderApiKey"
# ----- 사용자 설정 끝 -----

function Test-SecureStringEqual {
    param(
        [Parameter(Mandatory)]
        [System.Security.SecureString]$Left,

        [Parameter(Mandatory)]
        [System.Security.SecureString]$Right
    )

    if ($Left.Length -ne $Right.Length) {
        return $false
    }

    $leftPointer = [IntPtr]::Zero
    $rightPointer = [IntPtr]::Zero

    try {
        $leftPointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($Left)
        $rightPointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($Right)

        for ($index = 0; $index -lt $Left.Length; $index++) {
            if ([Runtime.InteropServices.Marshal]::ReadInt16($leftPointer, $index * 2) -ne [Runtime.InteropServices.Marshal]::ReadInt16($rightPointer, $index * 2)) {
                return $false
            }
        }

        return $true
    }
    finally {
        if ($leftPointer -ne [IntPtr]::Zero) {
            [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($leftPointer)
        }

        if ($rightPointer -ne [IntPtr]::Zero) {
            [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($rightPointer)
        }
    }
}

function Initialize-ExternalProviderSecretStore {
    Import-Module Microsoft.PowerShell.SecretManagement -RequiredVersion "1.1.2" -ErrorAction Stop
    Import-Module Microsoft.PowerShell.SecretStore -RequiredVersion "1.0.6" -ErrorAction Stop

    $vault = Get-SecretVault -Name $script:LocalSecretVaultName -ErrorAction SilentlyContinue
    if ($null -ne $vault) {
        return
    }

    Write-Host "외부 API provider용 SecretStore를 처음 설정합니다."

    while ($true) {
        $masterPassword = Read-Host "새 SecretStore 마스터 비밀번호" -AsSecureString
        $masterPasswordConfirmation = Read-Host "마스터 비밀번호 확인" -AsSecureString

        if ($masterPassword.Length -eq 0) {
            $masterPassword.Dispose()
            $masterPasswordConfirmation.Dispose()
            throw "마스터 비밀번호는 비워 둘 수 없습니다."
        }

        if (Test-SecureStringEqual -Left $masterPassword -Right $masterPasswordConfirmation) {
            break
        }

        $masterPassword.Dispose()
        $masterPasswordConfirmation.Dispose()
        Write-Warning "마스터 비밀번호가 일치하지 않습니다. 다시 입력해 주세요."
    }

    try {
        Register-SecretVault -Name $script:LocalSecretVaultName -ModuleName Microsoft.PowerShell.SecretStore -ErrorAction Stop
        Set-SecretStoreConfiguration -Scope CurrentUser -Authentication Password -PasswordTimeout -1 -Interaction Prompt -Password $masterPassword -Confirm:$false -ErrorAction Stop
        Unlock-SecretStore -Password $masterPassword -PasswordTimeout -1 -ErrorAction Stop
    }
    finally {
        $masterPassword.Dispose()
        $masterPasswordConfirmation.Dispose()
    }
}

function Get-ExternalProviderApiKey {
    Initialize-ExternalProviderSecretStore

    $secretInfo = Get-SecretInfo -Name $script:ExternalProviderSecretName -Vault $script:LocalSecretVaultName -ErrorAction Stop

    # 저장소 설정이 초기화됐더라도 현재 PowerShell 세션 동안 잠금 해제를 유지한다.
    Set-SecretStoreConfiguration -PasswordTimeout -1 -Interaction Prompt -Confirm:$false -ErrorAction Stop

    if ($null -eq $secretInfo) {
        $apiKey = Read-Host "외부 provider API 키 등록" -AsSecureString

        if ($apiKey.Length -eq 0) {
            $apiKey.Dispose()
            throw "API 키는 비워 둘 수 없습니다."
        }

        try {
            Set-Secret -Name $script:ExternalProviderSecretName -Secret $apiKey -Vault $script:LocalSecretVaultName -ErrorAction Stop
        }
        finally {
            $apiKey.Dispose()
        }
    }

    $secureApiKey = Get-Secret -Name $script:ExternalProviderSecretName -Vault $script:LocalSecretVaultName -ErrorAction Stop
    if ($secureApiKey -isnot [System.Security.SecureString]) {
        throw "SecretStore의 API 키가 SecureString 형식이 아닙니다."
    }

    $apiKeyPointer = [IntPtr]::Zero

    try {
        $apiKeyPointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureApiKey)
        return [Runtime.InteropServices.Marshal]::PtrToStringBSTR($apiKeyPointer)
    }
    finally {
        if ($apiKeyPointer -ne [IntPtr]::Zero) {
            [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($apiKeyPointer)
        }

        $secureApiKey.Dispose()
    }
}

function Invoke-CodexApiProject {
    param(
        [Parameter(Mandatory)]
        [string]$ProjectPath,

        [Parameter(ValueFromRemainingArguments = $true)]
        [string[]]$CodexArguments
    )

    $hadApiKey = Test-Path Env:AI_API_KEY
    $previousApiKey = $env:AI_API_KEY

    try {
        Set-Location -LiteralPath $ProjectPath

        $env:AI_API_KEY = Get-ExternalProviderApiKey

        & codex @CodexArguments
    }
    finally {
        if ($hadApiKey) {
            $env:AI_API_KEY = $previousApiKey
        }
        else {
            Remove-Item Env:AI_API_KEY -ErrorAction SilentlyContinue
        }
    }
}

function codex-api-main {
    Invoke-CodexApiProject -ProjectPath $script:CodexProjectPath @args
}
```

## 7. 최초 실행

프로필을 저장한 뒤 기존 PowerShell 프로세스를 완전히 종료하고 새 PowerShell을 연다. 이전 프로필에 평문 키 반환 함수가 있었다면 단순히 `. $PROFILE`로 다시 불러오기보다 프로세스를 종료해야 메모리에 남은 기존 함수도 사라진다.

외부 API provider 명령을 실행한다.

```powershell
codex-api-main
```

최초 실행에서는 다음 순서로 입력한다.

```text
외부 API provider용 SecretStore를 처음 설정합니다.
새 SecretStore 마스터 비밀번호: ********
마스터 비밀번호 확인: ********
외부 provider API 키 등록: ********
```

입력이 끝나면 API 키가 `ExternalProviderApiKey`라는 Secret으로 저장되고, 처음 요청한 Codex 명령이 이어서 실행된다.

필요한 경우 전달 인자도 그대로 Codex에 전달된다.

```powershell
codex-api-main --version
```

## 8. 이후 실행과 비밀번호 유지 시간

새 PowerShell 프로세스에서 처음 `codex-api-main`을 실행하면 SecretStore가 마스터 비밀번호를 요구한다.

```text
Vault LocalSecretStore requires a password.
Enter password:
********
```

`Enter password:` 다음 줄에 커서가 놓이는 것은 프로필이 추가한 개행이 아니라 SecretStore의 기본 대화형 프롬프트 형식이다.

프로필은 `PasswordTimeout`을 `-1`로 설정한다. 따라서 한 번 잠금을 해제하면 현재 PowerShell 프로세스가 종료될 때까지 비밀번호를 다시 입력하지 않는다. 새 Terminal 탭이나 새 PowerShell 창은 보통 별도 프로세스이므로 다시 입력해야 한다.

## 9. Secret 관리

### 등록 상태 확인

다음 명령은 Secret 이름과 형식만 보여 주며 API 키 값은 출력하지 않는다.

```powershell
Get-SecretVault
Get-SecretInfo -Name "ExternalProviderApiKey" -Vault "LocalSecretStore"
```

### API 키 변경

```powershell
$apiKey = Read-Host "새 외부 provider API 키" -AsSecureString

try {
    Set-Secret -Name "ExternalProviderApiKey" -Secret $apiKey -Vault "LocalSecretStore"
}
finally {
    $apiKey.Dispose()
}
```

### API 키만 제거

```powershell
Remove-Secret -Name "ExternalProviderApiKey" -Vault "LocalSecretStore"
```

다음 `codex-api-main` 실행 시 API 키를 다시 등록한다. Vault와 마스터 비밀번호는 유지된다.

### 마스터 비밀번호 변경

```powershell
Set-SecretStorePassword
```

### 저장소 전체 초기화

마스터 비밀번호를 잊었거나 저장 파일이 손상됐을 때만 사용한다.

```powershell
Reset-SecretStore
```

이 명령은 SecretStore 안의 모든 Secret과 설정을 제거한다. 다른 용도의 Secret이 함께 저장돼 있다면 모두 사라진다. 암호 파일을 탐색기에서 직접 삭제하면 Vault 등록 정보와 저장 데이터 상태가 어긋날 수 있으므로 공식 초기화 명령을 사용한다.

Windows의 SecretStore 데이터 경로:

```text
%LOCALAPPDATA%\Microsoft\PowerShell\secretmanagement\localstore\
```

SecretStore는 현재 사용자 범위의 단일 로컬 저장소를 사용한다. 같은 SecretStore 모듈을 여러 Vault 이름으로 등록해도 물리적으로 독립된 저장소가 여러 개 생기는 것은 아니다.

## 10. 검증

### 프로필 구문 검사

```powershell
$profileText = Get-Content -LiteralPath $PROFILE -Raw -Encoding UTF8
$tokens = $null
$errors = $null
[System.Management.Automation.Language.Parser]::ParseInput($profileText, [ref]$tokens, [ref]$errors) | Out-Null
$errors
```

아무것도 출력되지 않으면 PowerShell 구문 오류가 없는 것이다.

### 명령 등록 확인

```powershell
Get-Command codex-api-main |
    Select-Object Name, CommandType
```

`codex-api-main`이 `Function`으로 표시되어야 한다. 이는 Codex CLI의 내장 하위 명령이 아니라 PowerShell 프로필에 등록된 사용자 함수라는 뜻이다.

### 프로필의 토큰 형태 검사

```powershell
Select-String -LiteralPath $PROFILE -Pattern "sk-[A-Za-z0-9_-]{10,}"
```

아무것도 출력되지 않아야 한다. 이 검사는 흔한 토큰 형태를 찾는 정적 검사일 뿐 모든 종류의 비밀값을 탐지하는 것은 아니다.

### API-key 실행 경로 확인

새 PowerShell에서 다음을 실행한다.

```powershell
codex-api-main --version
```

SecretStore가 잠겨 있으면 마스터 비밀번호를 요구하고, 잠금 해제 후에는 저장된 API 키를 `AI_API_KEY`에 임시 설정하여 Codex를 실행해야 한다. Codex가 종료되면 프로필 함수가 기존 `AI_API_KEY` 값을 복원한다.

## 11. 보안 범위

마스터 비밀번호를 직접 지정하고 로컬 파일에 저장하지 않는 구성에서는, 누군가 SecretStore 암호 파일이나 사용자 파일을 복사했다는 사실만으로 API 키를 바로 복호화할 수 없다. Microsoft도 마스터 비밀번호를 로컬 컴퓨터에 저장하지 않는 구성을 권장한다.

다만 다음 시점에는 API 키가 사용 가능한 상태가 된다.

- 사용자가 마스터 비밀번호를 입력하는 동안
- Vault가 잠금 해제된 PowerShell 프로세스가 실행 중일 때
- Codex에 전달하기 위해 API 키가 일반 문자열과 환경변수로 변환된 동안

따라서 “Windows 로그인 계정이 노출되면 마스터 비밀번호 없이 즉시 복호화된다”는 설명은 정확하지 않다. 반면 공격자가 해당 계정에서 임의 코드를 지속적으로 실행할 수 있다면 프로필 변조, 입력 가로채기, 잠금 해제된 PowerShell 프로세스 접근 또는 Codex 실행 시점의 환경변수 관찰 등을 시도할 수 있다. 마스터 비밀번호는 저장 파일 유출과 잠긴 Vault에 대한 추가 보호층이지만, 이미 실행 중인 사용자 세션 전체가 장악된 상황까지 완전히 방어하지는 않는다.

`PasswordTimeout = -1`은 편리하지만 Vault가 잠금 해제된 시간을 PowerShell 프로세스 수명 전체로 늘린다. 보안을 더 우선하면 유한한 초 단위 값으로 변경한다.

```powershell
Set-SecretStoreConfiguration -PasswordTimeout 3600
```

API 키는 Codex 실행에 필요하므로 실행 중 메모리에서 완전히 없앨 수는 없다. 이 구성의 핵심 목적은 프로필, 설정 파일, 백업 파일 및 Git 기록에 평문 키가 지속적으로 남는 일을 방지하는 것이다.

## 12. 참고 문서

- [Codex Configuration Reference](https://developers.openai.com/codex/config-reference)
- [Microsoft SecretStore 보안 개념](https://learn.microsoft.com/en-us/powershell/utility-modules/secretmanagement/security-concepts?view=ps-modules)
- [Microsoft SecretStore 관리](https://learn.microsoft.com/en-us/powershell/utility-modules/secretmanagement/how-to/manage-secretstore?view=ps-modules)
