pipeline {
 agent any
 
 stages {
	stage('clone'){
		steps {
			echo 'Cloning source code'
			git branch:'main', url: 'https://github.com/noriadler49/food-ordering-system.git'
		}
	} // end clone
	stage ('Publish') {
		steps {
			echo 'public 2 runnig folder'
		//iisreset /stop // stop iis de ghi de file 
			bat 'xcopy "%WORKSPACE%\\publish" /E /Y /I /R "e:\\wwwroot\\food-ordering-system"'
 		}
	}

  } // end stages
}//end pipeline
