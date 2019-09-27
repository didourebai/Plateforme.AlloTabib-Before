"use strict";

app.controller("afficherCreneauParHController", function ($scope, $modalInstance, $window, status, heureDebut, heureFin, nomPrenomPatient, patients, telephone, cin, patientCin, evenementTitle, selectedDate, heuresDebut, filtredheuresFin, calendrierApiServices, estJourFerie, isCreated, informations, hideConfirmButton) {

    var praticien = $window.sessionStorage["userName"];
    calendrierApiServices.patients(praticien).then(
        // Case : Success.
                 function (results) {

                     $scope.patients = results.data.data.items;
                 },
                 // Case : Error.
                 function (error) {
                     $scope.errorMessageLoginTxt = "Problème d'affichage de calendrier pour récupérer la liste des patients liés, veuillez nous contacter. Equipe AlloTabib."
                 }
        );

    
    $scope.hideConfirmButton = hideConfirmButton;
    $scope.informations = informations;
    $scope.heureDebut = heureDebut;
    $scope.heureFin = heureFin;
    $scope.praticienCreneaux = status;
    $scope.nomPrenomPatient = nomPrenomPatient;
    $scope.telephone = telephone;
    $scope.cin = cin;
    $scope.patientCin = patientCin;
    $scope.evenementTitle = evenementTitle;
    $scope.selectedDate = selectedDate;
    $scope.heuresDebut = heuresDebut;
    $scope.filtredheuresFin = filtredheuresFin;
    $scope.estJourFerie = estJourFerie;
    $scope.isCreated = isCreated;
    $scope.patients = patients;
  
    $scope.confirmRdv = {
        heureDebut: heureDebut,
        heureFin: heureFin,
        patient: nomPrenomPatient,
        telephone: telephone,
        cin: cin,
        patientCin: patientCin

    };

    $scope.confirmerOuRejeterCreneau = function (cinpatient, heuredeb, statutc, patientcin) {
        var data = {
            cinpatient: cinpatient,
            heuredeb: heuredeb,
            statutc: statutc,
            patientcin: patientcin,
            etat:"update"
        };
        $modalInstance.close(data);
    };
    $scope.ajouterCreneau = function (confirmRdv) {
        confirmRdv.etat = "ajout";
        var dataConfor = confirmRdv;
        $modalInstance.close(dataConfor);
    };

    $scope.close = function () {
        $modalInstance.dismiss("cancel");
    };
});
;