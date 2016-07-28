'use strict';

/**
 * @ngdoc function
 * @name webApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the webApp
 */
angular.module('webApp')
  .controller('MainCtrl', function ($rootScope, $resource, NgTableParams) {
      $rootScope.pageTitle = 'View SQL Results';
      var vm = this;

      var url = 'http://localhost:59630/api/values';
      var sqlEndpoint = $resource(url);

      var sqlParams = {
          server: "sql server fqdn",
          database: "db name",
          userId: "username",
          password: "password",
          timeout: 60,
          queryString: "SELECT * FROM Customers;"
      };

      vm.tableParams = new NgTableParams({
        // initialParams
        count: 10
      }, {
        // initialSettings
        counts: [],
        getData: function (params) {
            // ajax request to api.
            return sqlEndpoint.get(sqlParams).$promise.then(function(data) {

                var results = data.Results.map(function(obj) {
                    var thisRow = {};
                    for (var name in obj.Data) {
                        if (obj.Data.hasOwnProperty(name)) {
                            thisRow[name] = {
                                key: name,
                                value: obj.Data[name]
                            };
                        }
                    }
                    return thisRow;
                });

                params.total(results.length); // recal. page nav controls
                return results;
            });
        }
      });
  });
