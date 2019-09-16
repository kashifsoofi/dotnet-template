import groovy.json.JsonOutput

node {
    def gitInfo

    def commonParameters = [
        projectName: 'template'
    ]
  
    stage('Build & Test') {
        echo 'Building and testing'
        gitInfo = checkout scm
        buildAndPushImages(commonParameters)
    }

    stage('Publish packages') {
        echo 'Publishing packages'
        publishPackages()
    }
}

def buildAndPushImages(parameters) {
    def registry = "my-registry:50001"
    echo "Building ${parameters.projectName}"

    def TemplateCoreApi = docker.build("${registry}/template-core-api:${BUILD_NUMBER}", "-f ./src/Template.WebApi/Dockerfile .")
    def TemplateCoreEndpoint = docker.build("${registry}/template-core-endpoint:${BUILD_NUMBER}", "-f ./src/Template.MessageProcessor/Dockerfile .")

    echo "Push image to registry"
    TemplateCoreApi.push()
    TemplateCoreEndpoint.push()
}

def publishPackages() {
    def registry = "my-registry:50001"
    def imageName = "${registry}/template-package-publisher:${BUILD_NUMBER}"
    def packagePublisher = docker.build(imageName, "--build-arg Version=0.1.0 -f ./Dockerfile.Publisher .")
	packagePublisher.run("--rm -v /usr/share/packages:/packages", "--source /packages")

	echo "Removing publisher image"
	docker.node {
		docker.script.sh "docker rmi ${imageName} -f"
	}
}