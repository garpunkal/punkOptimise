 angular.module("umbraco.resources")
    .factory("punkOptimiseResources", function ($http) {
        return {
            save: function (model) {
                return $http.post('backoffice/punkOptimise/punkOptimise/save', model);
            },

            isValid: function (id) {
                return $http.get('backoffice/punkOptimise/punkOptimise/isvalid?id=' + id);
            }
        };
    });   