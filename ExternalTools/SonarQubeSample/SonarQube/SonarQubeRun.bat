REM 사전에 SonarQube, SonarScanner 설치가 되어있어야 함
REM SonarScanner와 호환되는 해당 언어를 빌드할 툴이 있어야 함, Ex)SonarScanner.MsBuild.exe - MsBuild.exe 필요
REM SonarScanner, 빌드 툴(MsBuild)이 환경변수에 등록 되어있어야 함

REM SonarScanner 시작 /k:프로젝트 이름 /d:소나큐브 서버 URL /d:로그인 아이디 /d:비밀번호
SonarScanner.MSBuild.exe begin /k:"SonarQubeSample" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="admin" /d:sonar.password="admin"

REM 빌드툴 실행 path to solution.sln /t:빌드 옵션
msbuild C:\Users\User\Desktop\Project\Study\ExternalTools\SonarQubeSample\SonarQubeSample.sln /t:Rebuild

REM SonarScanner 종료 /d:로그인 아이디 /d:비밀번호
SonarScanner.MSBuild.exe end /d:sonar.login="admin" /d:sonar.password="admin" 