/// <reference path="../vsdoc/require.js" />
/// <reference path="../vsdoc/jquery-1.11.0.intellisense.js" />
/// <reference path="../lib/jquery-scrolltofixed.js" />
/// <reference path="../vsdoc/knockout-3.0.0.debug.js" />
/// <reference path="../ps/logger.js" />

"use strict";

define(["ps/logger",
    "jqueryui",
    "knockout",
    "lib/jquery-scrolltofixed"],
    function (logger, $, ko) {
        function ctor() {

            function bindModel(key) {
                require(["viewModels/" + key + "ViewModel"],
                    function (viewModel) {
                        if (typeof viewModel === "function") {
                            var vm = new viewModel(),
                                container = $.fn.pjaxScaffold.getContainer()[0];
                            ko.cleanNode(container);
                            ko.applyBindings(vm, container);
                            if (typeof vm.pageLoaded === "function") {
                                vm.pageLoaded();
                            }
                        }
                    });
            }

            function performLogout() {
                $.ajax({
                    type: "POST",
                    url: "/OAuth/Logout",
                    success: function (data, textStatus, jqXHR) {
                        initContainer("", "login");
                        //$("div.navbar").hide();
                        if ($.pjax) {
                            $.pjax({ url: "/", container: $.fn.pjaxScaffold.containerSelector });
                        } else {
                            document.location.reload();
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        logger.log("Error logging in ");
                        logger.log(errorThrown);
                        alert("error logging in");
                    }
                });
            }

            function initContainer(link, target) {
                var container = $.fn.pjaxScaffold.containerSelector,
                    determineActiveNav = function (url) {
                        var loc = url.toLowerCase();
                        if (loc.indexOf("account") === 0) {
                            return "#navAccount";
                        }
                        if (loc.indexOf("build") === 0) {
                            return "#navBuild";
                        }
                        return "#navHome";
                    },
                    navSelector = determineActiveNav(link);

                // Set navigation
                $("ul.nav li").removeClass("active");
                if (navSelector !== null) {
                    $(navSelector).addClass("active");
                }

                // Clear any intervals
                if (typeof $.fn.pjaxScaffold.clearIntervals === "function") {
                    $.fn.pjaxScaffold.clearIntervals();
                }

                // Bind href-specific asynchronous initialization$(document)
                $(document).on('ready pjax:success', container, function () {
                    bindModel(target); // Call initializers
                    $(document).off('ready pjax:success', container); // Unbind initialization
                });
            }

            function wireupPjaxEvents() {
                var container = $.fn.pjaxScaffold.getContainer();
                $(document)
                    .on("pjax:start", function () { container.stop(true, true).fadeOut(300); })
                    .on("pjax:end", function () { container.stop(true, true).fadeIn(300); });
                $('a[data-pjax]').on('click', function (event) {
                    var cs = $.fn.pjaxScaffold.containerSelector,
                        emptyRoute = 'login', // The function that will be called on domain's root
                        link = event.currentTarget.href.replace(/^.*\/\/[^\/]+\//, ''),
                        // Store current href without domain
                        target = (link === "" ? emptyRoute : link).toLowerCase();

                    initContainer(link, target);

                    // PJAX-load the new content
                    $.pjax.click(event, { container: cs });
                });
            }

            $.fn.pjaxScaffold = {
                containerSelector: "div.main-container div[data-pjax-container]",
                getContainer: function () {
                    return $($.fn.pjaxScaffold.containerSelector);
                }
            };

            $(document).ready(function () {
                //$("div.navbar").scrollToFixed();


                // Wire logout button
                $("#logout").click(function () {
                    // TODO: When user name is in header, show/hide this as necessary
                    performLogout();
                });

                //// Setup PJAX scaffold
                //if ($.support.pjax) {
                //    wireupPjaxEvents();
                //}
            });
        }

        return ctor;
    });