var app = angular.module("noiseCalcApp", [
]);

(function (module) {
    'use strict';

    module.controller("IndexController", [
        "$scope",
        function($scope) {

            $scope.firstPageSeen = false;

            $scope.firstPageHasBeenSeen = function () {
                $scope.firstPageSeen = true;
            }

        }
    ]);
}(angular.module("noiseCalcApp")));

