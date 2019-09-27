"use strict";

app.controller("calendrierModalController", function ($scope, $modalInstance, dateCourrante, rendezVousList) {

    $scope.rendezvous = [];
    for (var i = 0; i < rendezVousList.length; i++) {
        if (rendezVousList[i].statut != null && rendezVousList[i].statut != "" && rendezVousList[i].statut != undefined && rendezVousList[i].statut!="D") {
            $scope.rendezvous.push(rendezVousList[i]);
        }
    }
    
  $scope.dateCourrante = dateCourrante;

    $scope.close = function () {
        $modalInstance.dismiss("cancel");
    };
});
;