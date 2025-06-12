pipeline {
 agent any
 
 stages {
	stage('clone'){
		steps {
			echo 'Cloning source code'
			git branch:'main', url: 'https://github.com/noriadler49/food-ordering-system.git'
		}
	} // end clone
	
	stage('restore package') {
		steps
		{
			echo 'Restore package'
			bat 'dotnet restore'
		}
	}

stage ('build') {
		steps {
			echo 'build project netcore'
			bat 'dotnet build  --configuration Release'
		}
	}
	


	stage ('Publish') {
		steps {
			echo 'public 2 runnig folder'
		//iisreset /stop // stop iis de ghi de file 
			bat 'dotnet publish -c Release -o "e:\\wwwroot\\food-ordering-system"'

 		}
	}
	stage('Deploy to IIS') {
            steps {
                powershell '''
               
                Import-Module WebAdministration
                if (-not (Test-Path IIS:\\Sites\\MySite)) {
                    New-Website -Name "MySite" -Port 81 -PhysicalPath "e:\\wwwroot\\food-ordering-system"
                }
                '''
            }
        } // end deploy iis

  } // end stages
}//end pipeline
