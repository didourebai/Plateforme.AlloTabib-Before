'use strict';
app.controller('navigationController',
    function ($scope, $location, $rootScope,appSettings, $http, authService, $window) {

       

        $scope.isActive = function (path) {
            return $location.path().substr(0, path.length) == path;
        };

       
        init();
        function init() {
            $rootScope.authentication = $window.sessionStorage["userInfo"];
            $rootScope.userName = $window.sessionStorage["userName"];
            $rootScope.isAuth = $window.sessionStorage["isAuth"];
            $rootScope.nomPrenom = $window.sessionStorage["nomPrenom"];
            
            $rootScope.loggedInUser = $window.sessionStorage["userName"];

            $scope.$watch('isAuth', function () {

                $rootScope.isAuth = $window.sessionStorage["isAuth"];
                $rootScope.userName = $window.sessionStorage["userName"];
                $rootScope.nomPrenom = $window.sessionStorage["nomPrenom"];

            }, true);

        }

        //Save in cookie the email only
        document.cookie = "email=" + $scope.userName;
      
    });