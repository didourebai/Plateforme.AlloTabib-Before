'use strict';

app.controller('suivreMesRdvController', function ($scope, $window,$location, rendezvousApiServices) {

    var email = $window.sessionStorage["userName"];
    if (email == '' || email == null || email == undefined)
        $location.path('/loginpatient');
    
    initialise();
    $scope.rendezvousList = [];
    function initialise() {
        rendezvousApiServices.getAllRendezVous(email).then(
            // Case : Success.
            function (results) {
                var rdv = {
                    currentDate: "",
                    heureDebut: "",
                    praticienAdresse: "",
                    praticienNomPrenom: "",
                    praticienSpecialite: "",
                    statut:""
                };
                

                for (var i = 0; i < results.data.data.items.length; i++) {
                    rdv = results.data.data.items[i];
                    if (results.data.data.items[i].statut == "NC") {
                        rdv.statut = "Non confirmé";
                    } else if (results.data.data.items[i].statut == "C") {
                        rdv.statut = "Confirmé";
                    } else {
                        rdv.statut = "Rejeté";
                    }

                    $scope.rendezvousList.push(rdv);
                }
                //$scope.rendezvousList = results.data.data.items;
            },
            // Case : Error.
            function (error) {
                
            });
    }

    $scope.annulerRdv = function (rendezvous) {

    };


});