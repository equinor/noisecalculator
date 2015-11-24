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

            $scope.english = false;

            $scope.showEnglishInformation = function () {
                $scope.english = true;
            }

            $scope.showNorwegianInformation = function () {
                $scope.english = false;
            }
        }
    ]);
}(angular.module("noiseCalcApp")));

