pipeline {
 agent any
 
 stages {
	stage('clone'){
		steps {
			echo 'Cloning source code'
			git branch:'main', url: 'https://github.com/noriadler49/food-ordering-system.git'
		}
	} // end clone

  } // end stages
}//end pipeline
