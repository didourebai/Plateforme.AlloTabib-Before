'use strict';

app.controller('priseRendezVousController', function ($scope, $location, $window,$modal, rendezvousApiServices) {
    
   
    var userName = $window.sessionStorage["userName"];

    if (userName == '' || userName == null || userName == undefined)
        $location.path('/loginpatient');

    $scope.rendezVousDetails = {
        heurerdv: '',
        dateSelectionne: '',
        jour :'',
        praticien : null
    };

    initialize();
    
    function initialize() {
        
        if (objectFromOutSide != null && objectFromOutSide.heurerdv != '' && objectFromOutSide.dateSelectionne != '' && objectFromOutSide.jour != '' && objectFromOutSide.praticien != null) {
            $scope.rendezVousDetails = {
                heurerdv: objectFromOutSide.heurerdv,
                dateSelectionne: objectFromOutSide.dateSelectionne,
                jour: objectFromOutSide.jour,
                praticien: objectFromOutSide.praticien
            };

        } else {
            $location.path('/rendezvous');
        }
    }

    $scope.openPopupconfirmRdv = function (praticien, dateSelectionne, heurerdv) {
        console.log("open");
        var modalInstance = $modal.open({
            templateUrl: "app/views/inscription/confirmRendezVous.html",
            controller: "confirmRdvModalController",
            size: 'lg',
            resolve: {
                praticienCin: function () {
                    return praticien;
                },
                dateRendezVous: function () {
                    return dateSelectionne;
                },
                horaireRendezVous: function () {
                    return heurerdv;
                },
                specialiteRdv: function () {
                    return $scope.rendezVousDetails.praticien.specialite;
                },
                nomPrenomMe: function () {
                    return $scope.rendezVousDetails.praticien.nomPrenom;
                },
                adressePraticien : function() {
                    return ($scope.rendezVousDetails.praticien.adresse + " " + $scope.rendezVousDetails.praticien.gouvernerat + " " + $scope.rendezVousDetails.praticien.delegation);
                },
                telephonePraticien : function() {
                    return $scope.rendezVousDetails.praticien.telephone;
                }
            }
        });

        modalInstance.result.then(
            function (data) {

            }, function () {

            });
    };
    
    $scope.cancelAndLeave = function() {
        $location.path('/rendezvous');
    };
});