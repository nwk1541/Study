SonarScanner.MSBuild.exe begin /k:"SonarQubeSample" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="admin" /d:sonar.password="admin"

msbuild C:\Users\User\Desktop\Project\Study\ExternalTools\SonarQubeSample\SonarQubeSample.sln /t:Rebuild

SonarScanner.MSBuild.exe end /d:sonar.login="admin" /d:sonar.password="admin" 