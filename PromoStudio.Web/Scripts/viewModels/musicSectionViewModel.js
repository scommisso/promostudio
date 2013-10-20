/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.9.1.intellisense.js" />
/// <reference path="../lib/jquery-ui-effects-1.10.3.js" />
/// <reference path="../vsdoc/knockout-2.3.0.debug.js" />
/// <reference path="../models/enums.js" />
/// <reference path="../ps/logger.js" />
/// <reference path="../lib/ko.custom.js" />
/// <reference path="../ps/extensions.js" />
/// <reference path="../models/stockAudio.js" />

define(["models/stockAudio",
        "jquery",
        "knockout",
        "strings",
        "models/enums",
        "ps/logger",
        "bootstrap",
        "jqueryui"
    ],
    function (
        stockAudio,
        $,
        ko,
        strings,
        enums,
        logger) {
        return function (data, video) {
            var self = this,
                transitionTime = 350, /* from bootstrap-transitions */
                musicItem = null,
                customerTemplateScripts;
            data = data || {};
            video = video || {};

            self.AvailableMusic = ko.observableArray([]);
            self.SelectedMusic = ko.observable(null);
            self.IsSelected = function (music) {
                return self.SelectedMusic() === music;
            };
            self.SelectMusic = function (music) {
                self.SelectedMusic(music);
                musicItem.fk_CustomerVideoItemId(music.pk_StockAudioId());
            };
            
            self.IsCompleted = ko.computed(function () {
                var selectedMusic = self.SelectedMusic();
                return !!selectedMusic;
            });
            
            self.StartOpen = ko.observable(false);
            
            function loadData(customerTemplateScriptData, stockAudioData, videoData) {
                var id, songs, song, i;
                
                customerTemplateScripts = customerTemplateScriptData || [];
                loadAudioData(stockAudioData);
                loadVideoData(videoData);

                id = musicItem.fk_CustomerVideoItemId();
                if (id > 0) {
                    songs = self.AvailableMusic();
                    for (i = 0; i < songs.length; i++) {
                        song = songs[i];
                        if (song.pk_StockAudioId() === id) {
                            self.SelectMusic(song);
                            break;
                        }
                    }
                }
            }

            function loadAudioData(stockAudioData) {
                var music = [],
                    i, item;
                for (i = 0; i < stockAudioData.length; i++) {
                    item = stockAudioData[i];
                    item = new stockAudio(item);
                    music.push(item);
                }
                self.AvailableMusic(music);
            }

            function loadVideoData(videoData) {
                // Get existing voice acting item applied
                var items = videoData.Items(),
                    musicItems = $.grep(items, function (item) {
                        return item.fk_CustomerVideoItemTypeId() === 2;
                    });
                if (musicItems && musicItems.length > 0) {
                    musicItem = musicItems[0];
                }
            }
            
            function registerEvents() {
                $(function () {
                    var $elems = $("#musicCollapse .panel-heading .step-title,#musicCollapse .panel-heading .step-subtitle,#musicCollapse .panel-heading .step-done");
                    $('#musicPanel')
                        .on('show.bs.collapse', function() {
                            $elems.switchClass("collapsed", "opened", transitionTime);
                        })
                        .on('hide.bs.collapse', function () {
                            $elems.switchClass("opened", "collapsed", transitionTime);
                        });
                });
            }

            loadData(data.CustomerTemplateScripts, data.StockAudio, video);
            registerEvents();
        };
    });