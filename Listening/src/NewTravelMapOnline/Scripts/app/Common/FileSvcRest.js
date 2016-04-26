(function () {
    'use strict';

    angular.module('Common')
        .factory('FileSvcRest', function ($http, Upload) {

            var path = 'api/File/';

            return {
                upload: function (id, files) {
                    return Upload.upload({
                        url: path + id,
                        method: "PUT",
                        data: { files: files }
                    });
                },
                remove: function (fileName) {
                    return $http({
                        url: path + fileName,
                        method: 'DELETE'
                    });                    
                }
            };

        });
})();