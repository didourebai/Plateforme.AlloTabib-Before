'use strict';

app.controller('calendrierController', function ($scope, $location, $window, calendrierApiServices,contactApiServices, $modal) {
   
    var userName = $window.sessionStorage["userInfo"];

    if (userName == '' || userName == null || userName == undefined) {
        $location.path('/');
    }

    
    $scope.heures = [];
    $scope.heuresDebut = [];
    $scope.heuresFin = [];
    $scope.HasAnError = false;
    $scope.patients = [];
    
    $scope.estJourFerie = false;
    $scope.hasRendezVous = false;
    $scope.praticienCreneaux = true;
    $scope.hideConfirmButton = true;
    //getPatients();
    // init();
    function addZero(i) {
        if (i < 10) {
            i = "0" + i;
        }
        return i;
    }


    $scope.rendezvousList = [];

    $scope.openPopupImprimer = function (dateCurrent) {

        var modalInstance = $modal.open({
            templateUrl: "app/views/calendrier/detailscalendrier.html",
            controller: "calendrierModalController",
            size: 'lg',
            resolve: {
                rendezVousList: function () {
                    return $scope.rendezvousList;
                },
                dateCourrante: function () {
                    return dateCurrent;
                }
            }
        });

        modalInstance.result.then(
            function (data) {

            }, function () {

            });
    };

    function init() {
        $.blockUI();
        if ($scope.estJourFerie == false) {
            var dateSelect = $("#dateSelect").val();
            var praticien = $window.sessionStorage["userName"];

            calendrierApiServices.getRendezVous(praticien, dateSelect).then(
                // Case : Success.
                         function (results) {

                             var count = results.data.data.items.length;

                             if (count > 0) {
                               $scope.hasRendezVous = true;
                             }
                             else {
                                $scope.hasRendezVous = false;
                             }
                         },
                         // Case : Error.
                         function (error) {
                             $scope.errorMessageLoginTxt = "Problème de récupération des rendez vous un jour férié, veuillez nous contacter. Equipe AlloTabib.";
                         }
                );
        }
        getPatients();
        getRdvNonConfirme();
    };

    $scope.confirmOrRejectRdv = function (rdv, action) {
        console.log(rdv);
        console.log(action);
    };
    $scope.rdvsNonConfirme = [];
    function getRdvNonConfirme() {
        var praticien = $window.sessionStorage["userName"];
        calendrierApiServices.getAllRendezVousNonConfirmeOuRejete(praticien).then(
            function(results) {

                $scope.rdvsNonConfirme = results.data.data.items;
            },
            // Case : Error.
            function(error) {
                $scope.errorMessageLoginTxt = "Problème de récupération des rendez vous non confirmés. Veuillez contacter l'administrateur.";
            }
        );
    }
    $scope.isNotAvailable = function (statut) {
        if (status == 'ND') {
            {
                return true;
            }

        } else {
            return false;

        }
    };

    function getHeure(d) {
        var h = addZero(d.getHours());
        var m = addZero(d.getMinutes());
        var s = addZero(d.getSeconds());

        return (h + ":" + m + ":" + s);
    }
    $scope.filtredheuresFin = [];
    $scope.stateInputChanged = function (heureDeb) {

        $scope.filtredheuresFin = [];
        $scope.confirmRdv.heureFin = "";

        var indice = 0;
        for (var j = 0; j < $scope.heuresDebut.length - 1 ; j++) {

            if ($scope.heuresDebut[j] == heureDeb) {
                indice = j;
            }
        }

        for (var i = indice; i < $scope.heuresFin.length - 1 ; i++) {
            $scope.filtredheuresFin.push($scope.heuresFin[i]);

        }
    };


    function formattedDate(date) {

        var d = new Date(date || Date.now()),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

        var h = getHeure(d);

        //if (month.length < 2) month = '0' + month;
        //if (d.length < 2) d = '0' + d;

        var weekday = new Array(7);
        weekday[0] = " Dimanche ";
        weekday[1] = " Lundi ";
        weekday[2] = " Mardi ";
        weekday[3] = " Mercredi ";
        weekday[4] = " Jeudi ";
        weekday[5] = " Vendredi ";
        weekday[6] = " Samedi ";

        var n = weekday[d.getDay()];

        //get month
        var months = new Array(12);
        months[1] = " Janvier ";
        months[2] = " Février ";
        months[3] = " Mars ";
        months[4] = " Avril ";
        months[5] = " Mai ";
        months[6] = " Juin ";
        months[7] = " Juillet ";
        months[8] = " Aout ";
        months[9] = " Septembre ";
        months[10] = " Octobre ";
        months[11] = " Novembre ";
        months[12] = " Décembre ";

        var m = months[month];
        return [n, " " + day + " ", " " + m, year + " "].join('-');//+ "- " + h;
    };
    $scope.selectedDate = formattedDate(new Date());

    $scope.getDate = function () {
        var dateSelect = $("#dateSelect").val();
        $scope.dateCurrentSelected = dateSelect;
        var res = dateSelect.split("/");
        // var dateS = new Date(res[2], res[1], res[0]);

        var dateS = moment(dateSelect, "DD/MM/YYYY");
        $scope.selectedDate = formattedDate(dateS);
        init();
        construireCalendrier();
    };

    function deleteZero(hh) {
        var h = hh.substring(0, 1);
        if (h == '0')
            return hh.substring(1, 2);
        else {
            return hh;
        }
    }

    $scope.loadClassStatus = function (status) {

        if (status == 'ND') {
            {
                return 'statusND';
            }

        } else {
            $scope.isND = false;
            return 'statusD';
        }
    };

    $scope.dispose = false;
    $window.sessionStorage["statutRdv"] = "";

    $scope.updateCreneau = function (heure) {
        debugger;
        $scope.selectedHeure = heure;
        var dateSelect = $("#dateSelect").val();
        var praticien = $window.sessionStorage["userName"];
       
        if (heure.statut == "D") {
            $scope.evenementTitle = "Ajouter Rendez vous";
            //Il va ajouter un rendez vous à cette heure
            $scope.isCreated = true;
            $scope.praticienCreneaux = true;
            $scope.confirmRdv = {
                //heureDebut: heure.heureDebutCalendrier,
                //heureFin: heure.heureFinCalendrier,
                patient: heure.nomPrenomPatient,
                telephone: heure.telephonePatient,
                cin: heure.praticienCin,
                currentDate: dateSelect,
                patientCin: heure.patientCin
            };

            callPopUp($scope.confirmRdv, dateSelect, $scope.evenementTitle, $scope.heuresDebut, $scope.heuresFin, $scope.estJourFerie, $scope.isCreated, $scope.patients, $scope.praticienCreneaux, $scope.informations, $scope.hideConfirmButton);

            $scope.informations = "";
        }
        else {
            //if (heure.statut == 'ND')
            //{
            //   // $scope.nonDispo = false;

            //} else {
                //$scope.nonDispo = true;
                $scope.evenementTitle = "Rendez vous";
                $scope.isCreated = false;
                $scope.praticienCreneaux = false;
                // il peut être un rendez vous ou un creneau ajouté donc à vérifier
                $scope.statutRdv = "";
                calendrierApiServices.creneauAyantRdv(praticien, dateSelect, heure.heureDebutCalendrier).then(
                   // Case : Success.
                            function (results) {
                                console.log(results);
                                if (results.data.data.statut == 'C' || results.data.data.statut == 'R') {
                                    $window.sessionStorage["statutRdv"] = results.data.data.statut;
                                    $scope.isCreated = false;
                                    if (results.data.data.statut == 'C') {
                                        $scope.informations = "Vous avez confirmé ce rendez vous!!";
                                    }

                                    $scope.praticienCreneaux = false;
                                    $scope.hideConfirmButton = false;
                                }

                                else if (results.data.data.statut == 'NC') {
                                    $scope.informations = "Vous n'avez pas confirmé ni rejeté ce rendez vous, veuillez répondre le patient!";
                                    $scope.hideConfirmButton = true;
                                } else {

                                    $scope.informations = "";
                                    $scope.isCreated = true;
                                }

                               
                                callPopUp($scope.confirmRdv, dateSelect, $scope.evenementTitle, $scope.heuresDebut, $scope.heuresFin, $scope.estJourFerie, $scope.isCreated, $scope.patients, $scope.praticienCreneaux, $scope.informations, $scope.hideConfirmButton);
                            },
                            // Case : Error.
                            function (error) {
                                $scope.errorMessageLoginTxt = "Problème de suppression d'un jour férié, veuillez nous contacter. Equipe AlloTabib.";
                            }
                   );

                $scope.confirmRdv = {
                    heureDebut: heure.heureDebutCalendrier,
                    heureFin: heure.heureFinCalendrier,
                    patient: heure.nomPrenomPatient,
                    telephone: heure.telephonePatient,
                    cin: heure.praticienCin,
                    patientCin: heure.patientCin

                };


         //   }


            $scope.statutRdv = $window.sessionStorage["statutRdv"];
            if ($scope.statutRdv == 'C' || $scope.statutRdv == 'R') {
                $scope.praticienCreneaux = true;

            } else {
                $scope.praticienCreneaux = false;

            }




            setTimeout(function () {
                $('#selectedHeure').focus();
            }, 350);
            }
            
           
    };


    function callPopUp(confirmRdv, dateSelect, evenementTitle, heuresDebut, heuresFin, estJourFerie, isCreated, patients, praticienCreneaux, informations, hideConfirmButton)
    {
        //call popup here
        var modalInstance = $modal.open({
            templateUrl: "app/views/calendrier/afficherCreneauParHeure.html",
            controller: "afficherCreneauParHController",
            size: 'lg',
            resolve: {
                heureDebut: function () {
                    return confirmRdv.heureDebut;
                },
                heureFin: function () {
                    return confirmRdv.heureFin;
                },
                nomPrenomPatient: function () {
                    return confirmRdv.patient;
                },
                telephone: function () {
                    return confirmRdv.telephone;
                },
                cin: function () {
                    return confirmRdv.cin;
                },
                patientCin: function () {
                    return confirmRdv.patientCin;
                },
                evenementTitle: function () {
                    return evenementTitle;
                },
                selectedDate: function () {
                    return dateSelect;
                },
                heuresDebut: function () {
                    return heuresDebut;
                },
                filtredheuresFin: function () {
                    return heuresFin;
                },
                estJourFerie: function () {
                    return estJourFerie;
                },
                isCreated: function () {
                    return isCreated;
                },
                patients: function () {
                    return patients;
                },
                status: function () {
                    return praticienCreneaux;
                },
                informations: function () {
                    return informations;
                },
                hideConfirmButton: function () {
                    return hideConfirmButton;
                }

            }
        });

        modalInstance.result.then(
            function (data) {
                if (data.etat == "update")
                { $scope.confirmerOuRejeterCreneau(data.cinpatient, data.heuredeb, data.statutc, data.patientcin); }
                else
                { $scope.ajouterCreneau(data); }
            }, function () {
            });
    }
    function sendEmail(contactObject) {
        contactApiServices.sendEmail(contactObject)
           .then(
               // Case : Success.
               function (results) {
                   $.unblockUI();
                   $scope.newContact = {};

                   $scope.isSuccessfullyAdded = true;
                   $scope.addHasErrors = false;
               },
               // Case : Error.
               function (error) {
                   $.unblockUI();
                   //Afficher panel des erreurs
                   $scope.addHasErrors = true;
                   //ne pas afficher le message de succès
                   $scope.isSuccessfullyAdded = false;

                   // Add the errors messages.

                   if (error.status == 400) {
                       //if (error.data.errors[0].type == 'VALIDATION_FAILURE') {
                       //    $scope.addErrorType = "Erreur de validation";
                       //    $scope.errorMessageTxt = error.data.errors[0].message;
                       //}
                   }

                   if (error.status == 401) {
                       //$scope.addErrorMessage = JSON.parse(error.data);
                       //$scope.errorMessageTxt = "Non autorisé";
                   }

                   if (error.status == 500) {
                       //$scope.addErrorType = "Erreur lié au serveur";
                       //$scope.errorMessageTxt = error.data.errors[0].exception.Message;
                   }
                   //$scope.addErrorType = "Erreur anonyme";
                   //$scope.errorMessageTxt = error.data.toString();
               }
           );
    }
    $scope.confirmerOuRejeterCreneau = function (cinpatient, heuredeb, statutc, patientcin) {
        $.blockUI();
        var praticien = $window.sessionStorage["userName"];
        var dateSelect = $("#dateSelect").val();

        var rendezVous = {
            statut: statutc,
            patientCin: patientcin,
            praticienCin: praticien,
            currentDate: dateSelect,
            heureDebut: heuredeb
        };

       
        //juste un update au statut du rdv et une suppression du créneau
        calendrierApiServices.updateRendezVous(rendezVous).then(
              // Case : Success.
            function (results) {
                $.unblockUI();
                toastr.success("Vous avez mis à jour le rendez vous avec succès. Un Email sera envoyé au patient concerné pour l'informer de votre décision pour ce rendez vous.");
                var rendezVousInfo = results.data.data;


                var subject = "[AlloTabib]";
                var body = "Rendez vous";
               
                //send email
                if (statutc == 'R') {
                    $window.sessionStorage["statutRdv"] = "R";
                   //Rendez vous rejeté
                    subject = "[AlloTabib] Annulation de votre rendez vous";
                    body = "Bonjour,\nVotre rendez vous planifié le " + rendezVousInfo.currentDate + " à " + rendezVousInfo.heureDebut + " avec Dr." + rendezVousInfo.praticinNomPrenom + " , spécialité: " + rendezVousInfo.praticienSpecialite + " a été annulé suite à des engagements.\nVeuillez réserver un autre horaire sur notre site http://allotabib.net.\nCordialement\nEquipe AlloTabib.";

                } else {
                    $window.sessionStorage["statutRdv"] = "C";
                    //Rendez vous accepté
                    subject = "[AlloTabib] Confirmation de votre rendez vous";
                    body = "Bonjour,\nVotre demande de rendez-vous a été confirmé auprès du médecin.\nInformations rendez-vous : \n\nDate du rendez-vous : " + "le " + rendezVousInfo.currentDate + "à " + rendezVousInfo.heureDebut + "\n" + "Informations sur le spécialiste : \n" + rendezVousInfo.praticinNomPrenom + "\nSpécialité : " + rendezVousInfo.praticienSpecialite + "\nAdresse : " + rendezVousInfo.praticienAdresseDetaille + "\nTéléphone : " + rendezVousInfo.praticienTelephone + ".\nSi vous avez des questions veuillez nous contacter par e-mail: contact@allotabib.net!" + "\n\n\n---\nCordialement\nEquipe AlloTabib.";
                }
                
                //Envoyer un email au patient
                $scope.contactObject = {
                    from: "contact@allotabib.net",
                    to: rendezVousInfo.patientCin,
                    sujet: subject,
                    body: body
                };

                sendEmail($scope.contactObject);

                //Mail pour moi
                $scope.contactObject1 = {
                    from: "contact@allotabib.net",
                    to: "didourebai@gmail.com",
                    sujet: subject,
                    body: body
                };
                sendEmail($scope.contactObject1);
                $("#creneauModal").modal("hide");
                construireCalendrier();
                

            },
               // Case : Error.
            function (error) {
                $.unblockUI();
                toastr.error("Un problème a été survenue, veuillez essayer ultérieurement ou contacter AlloTabib.");
                $("#creneauModal").modal("hide");
            }

        );

    };

    $scope.ajouterJourFerie = function () {
        if ($scope.hasRendezVous == true && $scope.estJourFerie == false) {
            //on doit pas afficher le popup
            $("#confirmFerieModal").modal("show");
        }
        else {
            $.blockUI();
            //récupérer la date
            var dateSelect = $("#dateSelect").val();
            var praticien = $window.sessionStorage["userName"];
            var jourferie = {
                jourFerieNom: dateSelect,
                praticienEmail: praticien
            };

            if ($scope.estJourFerie == true) {
                calendrierApiServices.supprimerJourFerie(jourferie.jourFerieNom, jourferie.praticienEmail).then(
               // Case : Success.
                        function (results) {
                            $.unblockUI();
                            $scope.estJourFerie = false;
                            construireCalendrier();
                        },
                        // Case : Error.
                        function (error) {
                            $.unblockUI();
                            $scope.errorMessageLoginTxt = "Problème de supprimer un jour férié, veuillez nous contacter. Equipe AlloTabib."
                        }
               );
            }
            else {
                calendrierApiServices.ajouterJourFerie(jourferie).then(
                // Case : Success.
                         function (results) {
                             $.unblockUI();
                             $scope.estJourFerie = true;
                             construireCalendrier();
                         },
                         // Case : Error.
                         function (error) {
                             $.unblockUI();
                             $scope.errorMessageLoginTxt = "Problème de marquer un jour férié, veuillez nous contacter. Equipe AlloTabib."
                         }
                );
            }
        }
    };

    function addFerie() {
        $.blockUI();
        var dateSelect = $("#dateSelect").val();
        var praticien = $window.sessionStorage["userName"];
        var jourferie = {
            jourFerieNom: dateSelect,
            praticienEmail: praticien
        };
        calendrierApiServices.ajouterJourFerie(jourferie).then(
                // Case : Success.
                         function (results) {
                             $.unblockUI();
                             $scope.estJourFerie = true;
                             construireCalendrier();
                         },
                         // Case : Error.
                         function (error) {
                             $.unblockUI();
                             $scope.errorMessageLoginTxt = "Problème de marquer un jour férié, veuillez nous contacter. Equipe AlloTabib."
                         }
                );
    }

    $scope.confirmerFerieRendezVous = function () {
        addFerie();
        $("#confirmFerieModal").modal("hide");
    };
    function getPatients() {
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
    };

    function construireCalendrier() {
        $.blockUI();

        //insertion des heures par date

        var praticien = $window.sessionStorage["userName"];

        if ($("#dateSelect").val() != '' && $("#dateSelect").val() != undefined && praticien != '' && praticien != undefined) {
            calendrierApiServices.heures(praticien, $("#dateSelect").val()).then(
                     // Case : Success.
                     function (results) {
                         $scope.heures = [];
                         $scope.heuresDebut = [];
                         $scope.heuresFin = [];
                         $.unblockUI();
                         $scope.HasAnError = false;
                         var hours = results.data.data.calendrierPraticien;
                         $scope.rendezvousList = hours;

                         //results.data.data.calendrierPraticien[0].heureDebutCalendrier
                         for (var i = 0; i < hours.length; i++) {
                             $scope.heures.push(hours[i]);
                             if (hours[i].statut == "D") {
                                 $scope.heuresDebut.push(hours[i].heureDebutCalendrier);
                                 $scope.heuresFin.push(hours[i].heureFinCalendrier);
                             }
                         }
                     },
                     // Case : Error.
                     function (error) {
                         $.unblockUI();

                         $scope.HasAnError = true;
                         $scope.errorMessageLoginTxt = "Problème d'affichage de calendrier, veuillez nous contacter. Equipe AlloTabib.";
                     }
                 );

            //Verifier si le jour est un jour férié ou pas
            calendrierApiServices.estFerie($("#dateSelect").val(), praticien).then(
                         // Case : Success.
                         function (results) {


                             if (results.data.data.jourFerieNom == null) {
                                 $scope.jourFerie = "Ajouter férié";
                                 $scope.estJourFerie = false;
                             }
                             else {
                                 $scope.jourFerie = "Supprimer férié";
                                 $scope.estJourFerie = true;
                             }
                         },
                        // Case : Error.
                           function (error) {
                               $.unblockUI();
                           });
        }
        else {
            $.unblockUI();
            $scope.errorMessageLoginTxt = "Veuillez choisir une date pour avoir votre calendrier.";
        }

    };

    $scope.creneaux = [];
    $scope.ajouterCreneau = function (confirmRdv) {
        if (confirmRdv.heureDebut == '' || confirmRdv.heureFin == '') {

        }
        if ((confirmRdv.heureDebut == null || confirmRdv.heureFin == null)) {

        }
        else {

            $.blockUI();
            var dateSelect = $("#dateSelect").val();
            var praticien = $window.sessionStorage["userName"];

            var start = $scope.heuresDebut.indexOf(confirmRdv.heureDebut);
            var end = $scope.heuresFin.indexOf(confirmRdv.heureFin);

            for (var i = start; i < end + 1; i++) {
                var confirmCreneau = {
                    heureDebut: $scope.heuresDebut[i],
                    heureFin: $scope.heuresDebut[i + 1],
                    status: 'ND',
                    praticien: praticien,
                    currentDate: dateSelect,
                    commentaire: confirmRdv.patient
                };

                $scope.creneaux.push(confirmCreneau);


            }
            calendrierApiServices.ajouterCreneau($scope.creneaux).then(
                   // Case : Success.
                            function (results) {
                                $.unblockUI();
                                $scope.estJourFerie = true;
                                construireCalendrier();
                            },
                            // Case : Error.
                            function (error) {
                                $.unblockUI();
                                $scope.errorMessageLoginTxt = "Problème de marquer un jour férié, veuillez nous contacter. Equipe AlloTabib.";
                            }
                   );

            construireCalendrier();
            init();
            //Fermer le popup
            $("#creneauModal").modal("hide");
        }

    };

    $scope.imprimer = function () {
        var div = $("#calendrier");

    }
    $scope.loadClass = function (heure) {
        return 'blue-back';
    };
    $scope.addEvent = function (heure, valeur, obj) {
        $("#" + heure).css({ 'cursor': 'pointer' });

        if (valeur == '0')
            alert("0");
        else if (valeur == "1")
            alert("1");
        else if (valeur == "11")
            alert(heure);
        $(obj).css({ 'cursor': 'pointer' });
    };

    construireCalendrier();
    init();
});