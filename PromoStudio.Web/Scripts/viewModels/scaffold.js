define(["jquery", "knockout", "pjax", "bootstrap"],
    function ($, ko) {
        return function () {
            
            $.fn.pjaxScaffold = {
                containerSelector: "div.main-container div[data-pjax-container]",
                getContainer: function () {
                    return $($.fn.pjaxScaffold.containerSelector);
                }
            };

            function bindModel(key) {
                require(["viewModels/" + key + "ViewModel"],
                    function (viewModel) {
                        if (typeof viewModel === "function") {
                            var vm = new viewModel(),
                                container = $.fn.pjaxScaffold.getContainer()[0];
                            ko.cleanNode(container);
                            if (vm !== null) {
                                ko.applyBindings(vm, container);
                                if (typeof vm.pageLoaded === "function") {
                                    vm.pageLoaded();
                                }
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
                        $("div.navbar").hide();
                        $.pjax({ url: "/", container: $.fn.pjaxScaffold.containerSelector });
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.log("Error logging in ");
                        console.log(errorThrown);
                        alert("error logging in");
                    }
                });
            };

            function initContainer(link, target) {
                var container = $.fn.pjaxScaffold.containerSelector,
                    determineActiveNav = function (url) {
                        var loc = url.toLowerCase();
                        if (loc.indexOf("videos") === 0) { return "#navVideos"; }
                        if (loc.indexOf("resources") === 0) { return "#navResources"; }
                        if (loc.indexOf("build") === 0) { return "#navBuild"; }
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
            };

            function wireupPjaxEvents() {
                var container = $.fn.pjaxScaffold.getContainer();
                $(document)
                    .on("pjax:start", function () { container.stop(true, true).fadeOut(300); })
                    .on("pjax:end", function () { container.stop(true, true).fadeIn(300); })
                $('a[data-pjax]').on('click', function (event) {
                    var container = $.fn.pjaxScaffold.containerSelector,
                        emptyRoute = 'login', // The function that will be called on domain's root
                        link = event.currentTarget.href.replace(/^.*\/\/[^\/]+\//, ''),
                        // Store current href without domain
                        target = (link === "" ? emptyRoute : link).toLowerCase();

                    initContainer(link, target);

                    // PJAX-load the new content
                    $.pjax.click(event, { container: container });
                })
            };

            $(document).ready(function () {
                // Wire logout button
                $("#logout").click(function () {
                    // TODO: When user name is in header, show/hide this as necessary
                    performLogout();
                });

                // Setup PJAX scaffold
                if ($.support.pjax) {
                    wireupPjaxEvents();
                }
            });
        }
    });