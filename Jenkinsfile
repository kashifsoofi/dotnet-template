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
        dir('content') {
        publishPackages()
        }
    }
}

def buildAndPushImages(parameters) {
    def registry = "host.docker.local:5000" 
    echo "Building ${parameters.projectName}"

    def TemplateCoreApi = docker.build("${registry}/template-core-api:${BUILD_NUMBER}", "-f ./src/Template.WebApi/Dockerfile .")
    def TemplateCoreEndpoint = docker.build("${registry}/template-core-endpoint:${BUILD_NUMBER}", "-f ./src/Template.MessageProcessor/Dockerfile .")

    echo "Push image to registry"
    TemplateCoreApi.push()
    TemplateCoreEndpoint.push()
}

def publishPackages() {
    def registry = "host.docker.local:5000" 
    def imageName = "${registry}/template-package-publisher:${BUILD_NUMBER}"
    def packagePublisher = docker.build(imageName, "-f ./Dockerfile.Publisher .")
    docker.run(imageName, "-s /usr/share/packages")
}