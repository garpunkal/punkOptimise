(function () {
    "use strict";
    function punkOptimiseController(
        $scope,
        $route,
        entityResource,
        notificationsService,
        appState,
        navigationService,
        punkOptimiseResources) {

        var selectedNode = appState.getMenuState("currentNode");

        $scope.loading = false;
        $scope.isValid = false;
        $scope.isImage = true;
        $scope.nodeId = parseInt(selectedNode.id);
        $scope.nodeName = selectedNode.name;

        $scope.init = function () {
            $scope.loading = true;

            entityResource.getById($scope.nodeId, "Media")
                .then(function (media) {

                    if (media.metaData.ContentTypeAlias !== "Image" && media.metaData.ContentTypeAlias !== "umbracoMediaVectorGraphics") {
                        $scope.isImage = false;
                        return;
                    }
                    punkOptimiseResources.isValid($scope.nodeId)
                        .then(function (response) {
                            $scope.isValid = (response.data.resultType === 'Success');
                            $scope.loading = false;                            
                        }, function() {                            
                            notificationsService.error("Error", "Could not validate image.");
                        });
                }, function (response) {
                    console.log(response);
                    notificationsService.error("Error", "Could not validate image.");
                });
        };

        $scope.save = function () {
            $scope.loading = true;

            var saveModel = { id: $scope.nodeId };

            punkOptimiseResources.save(saveModel)
                .then(function (response) {

                    $scope.loading = false;
                    navigationService.hideDialog();

                    if (response.data.resultType === 'Success') {

                        notificationsService.success("Success", response.data.message);
                        $route.reload();
                    }
                    else
                        notificationsService.error("Error", response.data.message);
                }, function (response) {
                    notificationsService.error("Error", response.data.message);
                });
        };
    }
    angular.module('umbraco').controller("punkOptimise.Controller", punkOptimiseController);
})();