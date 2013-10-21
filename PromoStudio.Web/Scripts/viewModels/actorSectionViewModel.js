/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../lib/jquery-ui-effects-1.10.3.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../lib/ko.custom.js" />
/// <reference path="../ps/extensions.js" />

define(["models/customerVideoVoiceOver",
        "models/voiceActor",
        "viewModels/actorViewModel",
        "jquery",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "bootstrap",
        "jqueryui"
    ],
    function (
        customerVideoVoiceOver,
        voiceActor,
        actorViewModel,
        $,
        ko,
        strings,
        enums,
        logger) {
        return function (data, video) {
            var self = this,
                transitionTime = 350, /* from bootstrap-transitions */
                voiceOver = null,
                customerTemplateScripts;
            data = data || {};
            video = video || {};

            self.AvailableActors = ko.observableArray([]);
            self.SelectedActor = ko.observable(null);
            self.IsSelected = function (actor) {
                return self.SelectedActor() === actor;
            };
            self.SelectActor = function (actor) {
                if (self.IsSelected(actor)) {
                    self.SelectedActor(null);
                    voiceOver.fk_VoiceActorId(null);
                } else {
                    self.SelectedActor(actor);
                    voiceOver.fk_VoiceActorId(actor.pk_VoiceActorId());
                }
            };

            self.IsCompleted = ko.computed(function () {
                var selectedActor = self.SelectedActor();
                return !!selectedActor;
            });

            self.StartOpen = ko.observable(false);

            function loadData(customerTemplateScriptData, voiceActorData, videoData) {
                var id, actors, actor, i;
                
                customerTemplateScripts = customerTemplateScriptData || [];
                loadActorData(voiceActorData);
                loadVideoData(videoData);

                id = voiceOver.fk_VoiceActorId();
                if (id > 0) {
                    actors = self.AvailableActors();
                    for (i = 0; i < actors.length; i++) {
                        actor = actors[i];
                        if (actor.pk_VoiceActorId() === id) {
                            self.SelectActor(actor);
                            break;
                        }
                    }
                }
            }

            function loadActorData(voiceActorData) {
                var actor = [],
                    i, item;
                for (i = 0; i < voiceActorData.length; i++) {
                    item = voiceActorData[i];
                    item = new voiceActor(item);
                    item = new actorViewModel(item);
                    actor.push(item);
                }
                self.AvailableActors(actor);
            }

            function loadVideoData(videoData) {
                // Get voice over object
                voiceOver = videoData.VoiceOver();
            }

            function registerEvents() {
                $(function () {
                    var $elems = $("#actorCollapse .panel-heading .step-title,#actorCollapse .panel-heading .step-subtitle,#actorCollapse .panel-heading .step-done");
                    $('#actorPanel')
                        .on('show.bs.collapse', function () {
                            $elems.switchClass("collapsed", "opened", transitionTime);
                        })
                        .on('hide.bs.collapse', function () {
                            $elems.switchClass("opened", "collapsed", transitionTime);
                        });
                });
            }

            loadData(data.CustomerTemplateScripts, data.VoiceActors, video);
            registerEvents();
        };
    });