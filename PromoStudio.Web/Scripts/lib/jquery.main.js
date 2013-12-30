// page init
define(["jquery"], function(jQuery, undefined) {

    // initEditVideo
    function initEditVideo() {
	    var holder = jQuery('.info-video');
	    var frames = holder.find('.box');
	    frames.each(function() {
		    var self = jQuery(this);
		    var btn = self.find('.edit');
		    var box = self.find('.progress-bar li');

		    btn.on('click', function(event) {
			    event.preventDefault();
			    var that = jQuery(this);
			    if (!that.hasClass('active')) {
				    that.addClass('active');
				    box.removeClass('done');
					    box.addClass('active');
			    }
		    });

		    box.find('a').on('click', function(event) {
			    if (!btn.hasClass('active')) {
				    event.preventDefault();
			    }
		    });
	    });
    }

    // initEdit
    function initEdit() {
	    var holder = jQuery('.order-form-add');
	    var box = holder.find('.box');
	    box.each(function() {
		    var edit = box.find('.btn-edit a');
		    var input = box.find('input').attr('disabled', 'disabled');
		    edit.on('click', function(event) {
			    event.preventDefault();
			    var self = jQuery(this);
			    if (!self.parents('.btn-edit').hasClass('active')) {
				    self.parents('.btn-edit').addClass('active');
				    input.removeAttr('disabled')
			    } else {
				    self.parents('.btn-edit').removeClass('active');
				    input.attr('disabled', 'disabled');
			    }
			
		    });
	    });
    }

    // content tabs init
    function initTabs() {
	    jQuery('.tabset').contentTabs({
		    tabLinks: 'a'
	    });
    }

    // accordion menu init
    function initAccordion() {
	    jQuery('ul.accordion').slideAccordion({
		    opener: 'a.opener',
		    slider: 'div.slide',
		    animSpeed: 600
	    });
    }

    // fancybox modal popup init
    function initLightbox() {
	    jQuery('a.play, a[rel*="lightbox"]').each(function(){
	        var link = jQuery(this);
		    link.fancybox({
			    padding: 10,
			    margin: 0,
			    cyclic: false,
			    autoScale: true,
			    overlayShow: true,
			    overlayOpacity: 1,
			    overlayColor: '#efefef',
			    titlePosition: 'inside',
			    onComplete: function(box) {
				    if(link.attr('href').indexOf('#') === 0) {
					    jQuery('#fancybox-content').find('a.close').unbind('click.fb').bind('click.fb', function(e){
						    jQuery.fancybox.close();
						    e.preventDefault();
					    });
				    }
			    }
		    });
	    });
    }

    // form validation function
    function initValidation() {
	    var errorClass = 'error';
	    var successClass = 'success';
	    var regEmail = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
	    var regPhone = /^[0-9]+$/;

	    jQuery('form.validate-form').each(function(){
		    var form = jQuery(this).attr('novalidate', 'novalidate');
		    var successFlag = true;
		    var inputs = form.find('input, textarea, select');
		
		    // form validation function
		    function validateForm(e) {
			    successFlag = true;
			
			    inputs.each(checkField);
			
			    if(!successFlag) {
				    e.preventDefault();
			    }
		    }
		
		    // check field
		    function checkField(i, obj) {
			    var currentObject = jQuery(obj);
			    var currentParent = currentObject.closest('.row');
			
			    // not empty fields
			    if(currentObject.hasClass('required')) {
				    setState(currentParent, currentObject, !currentObject.val().length || currentObject.val() === currentObject.prop('defaultValue'));
			    }
			    // correct email fields
			    if(currentObject.hasClass('required-email')) {
				    setState(currentParent, currentObject, !regEmail.test(currentObject.val()));
			    }
			    // correct number fields
			    if(currentObject.hasClass('required-number')) {
				    setState(currentParent, currentObject, !regPhone.test(currentObject.val()));
			    }
			    // something selected
			    if(currentObject.hasClass('required-select')) {
				    setState(currentParent, currentObject, currentObject.get(0).selectedIndex === 0);
			    }
		    }
		
		    // set state
		    function setState(hold, field, error) {
			    hold.removeClass(errorClass).removeClass(successClass);
			    if(error) {
				    hold.addClass(errorClass);
				    field.one('focus',function(){hold.removeClass(errorClass).removeClass(successClass);});
				    successFlag = false;
			    } else {
				    hold.addClass(successClass);
			    }
		    }
		
		    // form event handlers
		    form.submit(validateForm);
	    });
    }


    /* Fancybox overlay fix */
    jQuery(function(){
	    // detect device type
	    var isTouchDevice = ('ontouchstart' in window) || window.DocumentTouch && document instanceof DocumentTouch;
	    var isWinPhoneDevice = navigator.msPointerEnabled && /MSIE 10.*Touch/.test(navigator.userAgent);

	    if(!isTouchDevice && !isWinPhoneDevice) {
		    // create <style> rules
		    var head = document.getElementsByTagName('head')[0],
			    style = document.createElement('style'),
			    rules = document.createTextNode('#fancybox-overlay'+'{'+
				    'position:fixed;'+
				    'top:0;'+
				    'left:0;'+
			    '}');

		    // append style element
		    style.type = 'text/css';
		    if(style.styleSheet) {
			    style.styleSheet.cssText = rules.nodeValue;
		    } else {
			    style.appendChild(rules);
		    }
		    head.appendChild(style);
	    }
    });

    // popups init
    function initPopups() {
	    jQuery('.popup-box').contentPopup({
		    mode: 'hover'
	    });
    }

    // add classes on hover/touch
    function initCustomHover() {
	    jQuery('.list-of-audio li').touchHover();
    }

    /*
     * jQuery Tabs plugin
     */
    ;(function($){
	    $.fn.contentTabs = function(o){
		    // default options
		    var options = $.extend({
			    activeClass:'active',
			    addToParent:false,
			    autoHeight:false,
			    autoRotate:false,
			    checkHash:false,
			    animSpeed:400,
			    switchTime:3000,
			    effect: 'none', // "fade", "slide"
			    tabLinks:'a',
			    attrib:'href',
			    event:'click'
		    },o);

		    return this.each(function(){
			    var tabset = $(this), tabs = $();
			    var tabLinks = tabset.find(options.tabLinks);
			    var tabLinksParents = tabLinks.parent();
			    var prevActiveLink = tabLinks.eq(0), currentTab, animating;
			    var tabHolder;

			    // handle location hash
			    if(options.checkHash && tabLinks.filter('[' + options.attrib + '="' + location.hash + '"]').length) {
				    (options.addToParent ? tabLinksParents : tabLinks).removeClass(options.activeClass);
				    setTimeout(function() {
					    window.scrollTo(0,0);
				    },1);
			    }

			    // init tabLinks
			    tabLinks.each(function(){
				    var link = $(this);
				    var href = link.attr(options.attrib);
				    var parent = link.parent();
				    href = href.substr(href.lastIndexOf('#'));

				    // get elements
				    var tab = $(href);
				    tabs = tabs.add(tab);
				    link.data('cparent', parent);
				    link.data('ctab', tab);

				    // find tab holder
				    if(!tabHolder && tab.length) {
					    tabHolder = tab.parent();
				    }

				    // show only active tab
				    var classOwner = options.addToParent ? parent : link;
				    if(classOwner.hasClass(options.activeClass) || (options.checkHash && location.hash === href)) {
					    classOwner.addClass(options.activeClass);
					    prevActiveLink = link; currentTab = tab;
					    tab.removeClass(tabHiddenClass).width('');
					    contentTabsEffect[options.effect].show({tab:tab, fast:true});
				    } else {
					    var tabWidth = tab.width();
					    if(tabWidth) {
						    tab.width(tabWidth);
					    }
					    tab.addClass(tabHiddenClass);
				    }

				    // event handler
				    link.bind(options.event, function(e){
					    if(link != prevActiveLink && !animating) {
						    switchTab(prevActiveLink, link);
						    prevActiveLink = link;
					    }
				    });
				    if(options.attrib === 'href') {
					    link.bind('click', function(e){
						    e.preventDefault();
					    });
				    }
			    });

			    // tab switch function
			    function switchTab(oldLink, newLink) {
				    animating = true;
				    var oldTab = oldLink.data('ctab');
				    var newTab = newLink.data('ctab');
				    prevActiveLink = newLink;
				    currentTab = newTab;

				    // refresh pagination links
				    (options.addToParent ? tabLinksParents : tabLinks).removeClass(options.activeClass);
				    (options.addToParent ? newLink.data('cparent') : newLink).addClass(options.activeClass);

				    // hide old tab
				    resizeHolder(oldTab, true);
				    contentTabsEffect[options.effect].hide({
					    speed: options.animSpeed,
					    tab:oldTab,
					    complete: function() {
						    // show current tab
						    resizeHolder(newTab.removeClass(tabHiddenClass).width(''));
						    contentTabsEffect[options.effect].show({
							    speed: options.animSpeed,
							    tab:newTab,
							    complete: function() {
								    if(!oldTab.is(newTab)) {
									    oldTab.width(oldTab.width()).addClass(tabHiddenClass);
								    }
								    animating = false;
								    resizeHolder(newTab, false);
								    autoRotate();
							    }
						    });
					    }
				    });
			    }

			    // holder auto height
			    function resizeHolder(block, state) {
				    var curBlock = block && block.length ? block : currentTab;
				    if(options.autoHeight && curBlock) {
					    tabHolder.stop();
					    if(state === false) {
						    tabHolder.css({height:''});
					    } else {
						    var origStyles = curBlock.attr('style');
						    curBlock.show().css({width:curBlock.width()});
						    var tabHeight = curBlock.outerHeight(true);
						    if(!origStyles) curBlock.removeAttr('style'); else curBlock.attr('style', origStyles);
						    if(state === true) {
							    tabHolder.css({height: tabHeight});
						    } else {
							    tabHolder.animate({height: tabHeight}, {duration: options.animSpeed});
						    }
					    }
				    }
			    }
			    if(options.autoHeight) {
				    $(window).bind('resize orientationchange', function(){
					    tabs.not(currentTab).removeClass(tabHiddenClass).show().each(function(){
						    var tab = jQuery(this), tabWidth = tab.css({width:''}).width();
						    if(tabWidth) {
							    tab.width(tabWidth);
						    }
					    }).hide().addClass(tabHiddenClass);

					    resizeHolder(currentTab, false);
				    });
			    }

			    // autorotation handling
			    var rotationTimer;
			    function nextTab() {
				    var activeItem = (options.addToParent ? tabLinksParents : tabLinks).filter('.' + options.activeClass);
				    var activeIndex = (options.addToParent ? tabLinksParents : tabLinks).index(activeItem);
				    var newLink = tabLinks.eq(activeIndex < tabLinks.length - 1 ? activeIndex + 1 : 0);
				    prevActiveLink = tabLinks.eq(activeIndex);
				    switchTab(prevActiveLink, newLink);
			    }
			    function autoRotate() {
				    if(options.autoRotate && tabLinks.length > 1) {
					    clearTimeout(rotationTimer);
					    rotationTimer = setTimeout(function() {
						    if(!animating) {
							    nextTab();
						    } else {
							    autoRotate();
						    }
					    }, options.switchTime);
				    }
			    }
			    autoRotate();
		    });
	    };

	    // add stylesheet for tabs on DOMReady
	    var tabHiddenClass = 'js-tab-hidden';
	    $(function() {
		    var tabStyleSheet = $('<style type="text/css">')[0];
		    var tabStyleRule = '.'+tabHiddenClass;
		    tabStyleRule += '{position:absolute !important;left:-9999px !important;top:-9999px !important;display:block !important}';
		    if (tabStyleSheet.styleSheet) {
			    tabStyleSheet.styleSheet.cssText = tabStyleRule;
		    } else {
			    tabStyleSheet.appendChild(document.createTextNode(tabStyleRule));
		    }
		    $('head').append(tabStyleSheet);
	    });

	    // tab switch effects
	    var contentTabsEffect = {
		    none: {
			    show: function(o) {
				    o.tab.css({display:'block'});
				    if(o.complete) o.complete();
			    },
			    hide: function(o) {
				    o.tab.css({display:'none'});
				    if(o.complete) o.complete();
			    }
		    },
		    fade: {
			    show: function(o) {
				    if(o.fast) o.speed = 1;
				    o.tab.fadeIn(o.speed);
				    if(o.complete) setTimeout(o.complete, o.speed);
			    },
			    hide: function(o) {
				    if(o.fast) o.speed = 1;
				    o.tab.fadeOut(o.speed);
				    if(o.complete) setTimeout(o.complete, o.speed);
			    }
		    },
		    slide: {
			    show: function(o) {
				    var tabHeight = o.tab.show().css({width:o.tab.width()}).outerHeight(true);
				    var tmpWrap = $('<div class="effect-div">').insertBefore(o.tab).append(o.tab);
				    tmpWrap.css({width:'100%', overflow:'hidden', position:'relative'}); o.tab.css({marginTop:-tabHeight,display:'block'});
				    if(o.fast) o.speed = 1;
				    o.tab.animate({marginTop: 0}, {duration: o.speed, complete: function(){
					    o.tab.css({marginTop: '', width: ''}).insertBefore(tmpWrap);
					    tmpWrap.remove();
					    if(o.complete) o.complete();
				    }});
			    },
			    hide: function(o) {
				    var tabHeight = o.tab.show().css({width:o.tab.width()}).outerHeight(true);
				    var tmpWrap = $('<div class="effect-div">').insertBefore(o.tab).append(o.tab);
				    tmpWrap.css({width:'100%', overflow:'hidden', position:'relative'});

				    if(o.fast) o.speed = 1;
				    o.tab.animate({marginTop: -tabHeight}, {duration: o.speed, complete: function(){
					    o.tab.css({display:'none', marginTop:'', width:''}).insertBefore(tmpWrap);
					    tmpWrap.remove();
					    if(o.complete) o.complete();
				    }});
			    }
		    }
	    };
    }(jQuery));

    /*
     * jQuery Accordion plugin
     */
    ;(function($){
	    $.fn.slideAccordion = function(opt){
		    // default options
		    var options = $.extend({
			    addClassBeforeAnimation: false,
			    activeClass:'active',
			    opener:'.opener',
			    slider:'.slide',
			    animSpeed: 300,
			    collapsible:true,
			    event:'click'
		    },opt);

		    return this.each(function(){
			    // options
			    var accordion = $(this);
			    var items = accordion.find(':has('+options.slider+')');

			    items.each(function(){
				    var item = $(this);
				    var opener = item.find(options.opener);
				    var slider = item.find(options.slider);
				    opener.bind(options.event, function(e){
					    if(!slider.is(':animated')) {
						    if(item.hasClass(options.activeClass)) {
							    if(options.collapsible) {
								    slider.slideUp(options.animSpeed, function(){
									    hideSlide(slider);
									    item.removeClass(options.activeClass);
								    }).fadeOut({
									    duration: options.animSpeed,
									    queue: false
								    });
							    }
						    } else {
							    // show active
							    var levelItems = item.siblings('.'+options.activeClass);
							    var sliderElements = levelItems.find(options.slider);
							    item.addClass(options.activeClass);
							    showSlide(slider).hide().slideDown(options.animSpeed).css('opacity', 0).animate({
								    opacity: 1
							    }, {
								    duration: options.animSpeed,
								    queue: false
							    });
						
							    // collapse others
							    sliderElements.slideUp(options.animSpeed, function(){
								    levelItems.removeClass(options.activeClass);
								    hideSlide(sliderElements);
							    }).fadeOut({
								    duration: options.animSpeed,
								    queue: false
							    });;
						    }
					    }
					    e.preventDefault();
				    });
				    if(item.hasClass(options.activeClass)) showSlide(slider); else hideSlide(slider);
			    });
		    });
	    };

	    // accordion slide visibility
	    var showSlide = function(slide) {
		    return slide.css({position:'', top: '', left: '', width: '' });
	    };
	    var hideSlide = function(slide) {
		    return slide.show().css({position:'absolute', top: -9999, left: -9999, width: slide.width() });
	    };
    }(jQuery));

    /*
     * Popups plugin
     */
    ;(function($) {
	    function ContentPopup(opt) {
		    this.options = $.extend({
			    holder: null,
			    popup: '.popup',
			    btnOpen: '.open',
			    btnClose: '.close',
			    openClass: 'popup-active',
			    clickEvent: 'click',
			    mode: 'click',
			    hideOnClickLink: true,
			    hideOnClickOutside: true,
			    delay: 50
		    }, opt);
		    if(this.options.holder) {
			    this.holder = $(this.options.holder);
			    this.init();
		    }
	    }
	    ContentPopup.prototype = {
		    init: function() {
			    this.findElements();
			    this.attachEvents();
		    },
		    findElements: function() {
			    this.popup = this.holder.find(this.options.popup);
			    this.btnOpen = this.holder.find(this.options.btnOpen);
			    this.btnClose = this.holder.find(this.options.btnClose);
		    },
		    attachEvents: function() {
			    // handle popup openers
			    var self = this;
			    this.clickMode = isTouchDevice || (self.options.mode === self.options.clickEvent);

			    if(this.clickMode) {
				    // handle click mode
				    this.btnOpen.bind(self.options.clickEvent, function(e) {
					    if(self.holder.hasClass(self.options.openClass)) {
						    if(self.options.hideOnClickLink) {
							    self.hidePopup();
						    }
					    } else {
						    self.showPopup();
					    }
					    e.preventDefault();
				    });

				    // prepare outside click handler
				    this.outsideClickHandler = this.bind(this.outsideClickHandler, this);
			    } else {
				    // handle hover mode
				    var timer, delayedFunc = function(func) {
					    clearTimeout(timer);
					    timer = setTimeout(function() {
						    func.call(self);
					    }, self.options.delay);
				    };
				    this.btnOpen.bind('mouseover', function() {
					    delayedFunc(self.showPopup);
				    }).bind('mouseout', function() {
					    delayedFunc(self.hidePopup);
				    });
				    this.popup.bind('mouseover', function() {
					    delayedFunc(self.showPopup);
				    }).bind('mouseout', function() {
					    delayedFunc(self.hidePopup);
				    });
			    }

			    // handle close buttons
			    this.btnClose.bind(self.options.clickEvent, function(e) {
				    self.hidePopup();
				    e.preventDefault();
			    });
		    },
		    outsideClickHandler: function(e) {
			    // hide popup if clicked outside
			    var currentNode = (e.changedTouches ? e.changedTouches[0] : e).target;
			    if(!$(currentNode).parents().filter(this.holder).length) {
				    this.hidePopup();
			    }
		    },
		    showPopup: function() {
			    // reveal popup
			    this.holder.addClass(this.options.openClass);
			    this.popup.css({display:'block'});

			    // outside click handler
			    if(this.clickMode && this.options.hideOnClickOutside && !this.outsideHandlerActive) {
				    this.outsideHandlerActive = true;
				    $(document).bind('click touchstart', this.outsideClickHandler);
			    }
		    },
		    hidePopup: function() {
			    // hide popup
			    this.holder.removeClass(this.options.openClass);
			    this.popup.css({display:'none'});

			    // outside click handler
			    if(this.clickMode && this.options.hideOnClickOutside && this.outsideHandlerActive) {
				    this.outsideHandlerActive = false;
				    $(document).unbind('click touchstart', this.outsideClickHandler);
			    }
		    },
		    bind: function(f, scope, forceArgs){
			    return function() {return f.apply(scope, forceArgs ? [forceArgs] : arguments);};
		    }
	    };

	    // detect touch devices
	    var isTouchDevice = /MSIE 10.*Touch/.test(navigator.userAgent) || ('ontouchstart' in window) || window.DocumentTouch && document instanceof DocumentTouch;

	    // jQuery plugin interface
	    $.fn.contentPopup = function(opt) {
		    return this.each(function() {
			    new ContentPopup($.extend(opt, {holder: this}));
		    });
	    };
    }(jQuery));

    /*
     * Mobile hover plugin
     */
    ;(function($){

	    // detect device type
	    var isTouchDevice = ('ontouchstart' in window) || window.DocumentTouch && document instanceof DocumentTouch,
		    isWinPhoneDevice = navigator.msPointerEnabled && /MSIE 10.*Touch/.test(navigator.userAgent);

	    // define events
	    var eventOn = (isTouchDevice && 'touchstart') || (isWinPhoneDevice && 'MSPointerDown') || 'mouseenter',
		    eventOff = (isTouchDevice && 'touchend') || (isWinPhoneDevice && 'MSPointerUp') || 'mouseleave';

	    // event handlers
	    var toggleOn, toggleOff, preventHandler;
	    if(isTouchDevice || isWinPhoneDevice) {
		    // prevent click handler
		    preventHandler = function(e) {
			    e.preventDefault();
		    };

		    // touch device handlers
		    toggleOn = function(e) {
			    var options = e.data, element = $(this);

			    var toggleOff = function(e) {
				    var target = $(e.target);
				    if (!target.is(element) && !target.closest(element).length) {
					    element.removeClass(options.hoverClass);
					    element.off('click', preventHandler);
					    if(options.onLeave) options.onLeave(element);
					    $(document).off(eventOn, toggleOff);
				    }
			    };

			    if(!element.hasClass(options.hoverClass)) {
				    element.addClass(options.hoverClass);
				    element.one('click', preventHandler);
				    $(document).on(eventOn, toggleOff);
				    if(options.onHover) options.onHover(element);
			    }
		    };
	    } else {
		    // desktop browser handlers
		    toggleOn = function(e) {
		        var options = e.data, element = $(this);
		        element.parent().children().removeClass(options.hoverClass);
			    element.addClass(options.hoverClass);
			    $(options.context).on(eventOff, options.selector, options, toggleOff);
			    if(options.onHover) options.onHover(element);
		    };
		    toggleOff = function(e) {
		        var options = e.data, element = $(this);
		        element.parent().children().removeClass(options.hoverClass);
			    $(options.context).off(eventOff, options.selector, toggleOff);
			    if(options.onLeave) options.onLeave(element);
		    };
	    }

	    // jQuery plugin
	    $.fn.touchHover = function(opt) {
		    var options = $.extend({
			    context: this.context,
			    selector: this.selector,
			    hoverClass: 'hover'
		    }, opt);

		    $(this.context).on(eventOn, this.selector, options, toggleOn);
		    return this;
	    };
    }(jQuery));

    /*! http://mths.be/placeholder v2.0.6 by @mathias */
    ;(function(window, document, $) {

	    var isInputSupported = 'placeholder' in document.createElement('input'),
	        isTextareaSupported = 'placeholder' in document.createElement('textarea'),
	        prototype = $.fn,
	        valHooks = $.valHooks,
	        hooks,
	        placeholder;
	    if(navigator.userAgent.indexOf('Opera/') != -1) {
		    isInputSupported = isTextareaSupported = false;
	    }
	    if (isInputSupported && isTextareaSupported) {

		    placeholder = prototype.placeholder = function() {
			    return this;
		    };

		    placeholder.input = placeholder.textarea = true;

	    } else {

		    placeholder = prototype.placeholder = function() {
			    var $this = this;
			    $this
				    .filter((isInputSupported ? 'textarea' : ':input') + '[placeholder]')
				    .not('.placeholder')
				    .bind({
					    'focus.placeholder': clearPlaceholder,
					    'blur.placeholder': setPlaceholder
				    })
				    .data('placeholder-enabled', true)
				    .trigger('blur.placeholder');
			    return $this;
		    };

		    placeholder.input = isInputSupported;
		    placeholder.textarea = isTextareaSupported;

		    hooks = {
			    'get': function(element) {
				    var $element = $(element);
				    return $element.data('placeholder-enabled') && $element.hasClass('placeholder') ? '' : element.value;
			    },
			    'set': function(element, value) {
				    var $element = $(element);
				    if (!$element.data('placeholder-enabled')) {
					    return element.value = value;
				    }
				    if (value == '') {
					    element.value = value;
					    // Issue #56: Setting the placeholder causes problems if the element continues to have focus.
					    if (element != document.activeElement) {
						    // We can’t use `triggerHandler` here because of dummy text/password inputs :(
						    setPlaceholder.call(element);
					    }
				    } else if ($element.hasClass('placeholder')) {
					    clearPlaceholder.call(element, true, value) || (element.value = value);
				    } else {
					    element.value = value;
				    }
				    // `set` can not return `undefined`; see http://jsapi.info/jquery/1.7.1/val#L2363
				    return $element;
			    }
		    };

		    isInputSupported || (valHooks.input = hooks);
		    isTextareaSupported || (valHooks.textarea = hooks);

		    $(function() {
			    // Look for forms
			    $(document).delegate('form', 'submit.placeholder', function() {
				    // Clear the placeholder values so they don’t get submitted
				    var $inputs = $('.placeholder', this).each(clearPlaceholder);
				    setTimeout(function() {
					    $inputs.each(setPlaceholder);
				    }, 10);
			    });
		    });

		    // Clear placeholder values upon page reload
		    $(window).bind('beforeunload.placeholder', function() {
			    $('.placeholder').each(function() {
				    this.value = '';
			    });
		    });

	    }

	    function args(elem) {
		    // Return an object of element attributes
		    var newAttrs = {},
		        rinlinejQuery = /^jQuery\d+$/;
		    $.each(elem.attributes, function(i, attr) {
			    if (attr.specified && !rinlinejQuery.test(attr.name)) {
				    newAttrs[attr.name] = attr.value;
			    }
		    });
		    return newAttrs;
	    }

	    function clearPlaceholder(event, value) {
		    var input = this,
		        $input = $(input),
		        hadFocus;
		    if (input.value == $input.attr('placeholder') && $input.hasClass('placeholder')) {
			    hadFocus = input == document.activeElement;
			    if ($input.data('placeholder-password')) {
				    $input = $input.hide().next().show().attr('id', $input.removeAttr('id').data('placeholder-id'));
				    // If `clearPlaceholder` was called from `$.valHooks.input.set`
				    if (event === true) {
					    return $input[0].value = value;
				    }
				    $input.focus();
			    } else {
				    input.value = '';
				    $input.removeClass('placeholder');
			    }
			    hadFocus && input.select();
		    }
	    }

	    function setPlaceholder() {
		    var $replacement,
		        input = this,
		        $input = $(input),
		        $origInput = $input,
		        id = this.id;
		    if (input.value == '') {
			    if (input.type == 'password') {
				    if (!$input.data('placeholder-textinput')) {
					    try {
						    $replacement = $input.clone().attr({ 'type': 'text' });
					    } catch(e) {
						    $replacement = $('<input>').attr($.extend(args(this), { 'type': 'text' }));
					    }
					    $replacement
						    .removeAttr('name')
						    .data({
							    'placeholder-password': true,
							    'placeholder-id': id
						    })
						    .bind('focus.placeholder', clearPlaceholder);
					    $input
						    .data({
							    'placeholder-textinput': $replacement,
							    'placeholder-id': id
						    })
						    .before($replacement);
				    }
				    $input = $input.removeAttr('id').hide().prev().attr('id', id).show();
				    // Note: `$input[0] != input` now!
			    }
			    $input.addClass('placeholder');
			    $input[0].value = $input.attr('placeholder');
		    } else {
			    $input.removeClass('placeholder');
		    }
	    }

    }(this, document, jQuery));

    /*
     * JavaScript Custom Forms Module
     */
    jcf = {
	    // global options
	    modules: {},
	    plugins: {},
	    baseOptions: {
		    unselectableClass:'jcf-unselectable',
		    labelActiveClass:'jcf-label-active',
		    labelDisabledClass:'jcf-label-disabled',
		    classPrefix: 'jcf-class-',
		    hiddenClass:'jcf-hidden',
		    focusClass:'jcf-focus',
		    wrapperTag: 'div'
	    },
	    // replacer function
	    customForms: {
		    setOptions: function(obj) {
			    for(var p in obj) {
				    if(obj.hasOwnProperty(p) && typeof obj[p] === 'object') {
					    jcf.lib.extend(jcf.modules[p].prototype.defaultOptions, obj[p]);
				    }
			    }
		    },
		    replaceAll: function(context) {
			    for(var k in jcf.modules) {
				    var els = jcf.lib.queryBySelector(jcf.modules[k].prototype.selector, context);
				    for(var i = 0; i<els.length; i++) {
					    if(els[i].jcf) {
						    // refresh form element state
						    els[i].jcf.refreshState();
					    } else {
						    // replace form element
						    if(!jcf.lib.hasClass(els[i], 'default') && jcf.modules[k].prototype.checkElement(els[i])) {
							    new jcf.modules[k]({
								    replaces:els[i]
							    });
						    }
					    }
				    }
			    }
		    },
		    refreshAll: function(context) {
			    for(var k in jcf.modules) {
				    var els = jcf.lib.queryBySelector(jcf.modules[k].prototype.selector, context);
				    for(var i = 0; i<els.length; i++) {
					    if(els[i].jcf) {
						    // refresh form element state
						    els[i].jcf.refreshState();
					    }
				    }
			    }
		    },
		    refreshElement: function(obj) {
			    if(obj && obj.jcf) {
				    obj.jcf.refreshState();
			    }
		    },
		    destroyAll: function() {
			    for(var k in jcf.modules) {
				    var els = jcf.lib.queryBySelector(jcf.modules[k].prototype.selector);
				    for(var i = 0; i<els.length; i++) {
					    if(els[i].jcf) {
						    els[i].jcf.destroy();
					    }
				    }
			    }
		    }
	    },
	    // detect device type
	    isTouchDevice: ('ontouchstart' in window) || window.DocumentTouch && document instanceof DocumentTouch,
	    isWinPhoneDevice: navigator.msPointerEnabled && /MSIE 10.*Touch/.test(navigator.userAgent),
	    // define base module
	    setBaseModule: function(obj) {
		    jcf.customControl = function(opt){
			    this.options = jcf.lib.extend({}, jcf.baseOptions, this.defaultOptions, opt);
			    this.init();
		    };
		    for(var p in obj) {
			    jcf.customControl.prototype[p] = obj[p];
		    }
	    },
	    // add module to jcf.modules
	    addModule: function(obj) {
		    if(obj.name){
			    // create new module proto class
			    jcf.modules[obj.name] = function(){
				    jcf.modules[obj.name].superclass.constructor.apply(this, arguments);
			    }
			    jcf.lib.inherit(jcf.modules[obj.name], jcf.customControl);
			    for(var p in obj) {
				    jcf.modules[obj.name].prototype[p] = obj[p]
			    }
			    // on create module
			    jcf.modules[obj.name].prototype.onCreateModule();
			    // make callback for exciting modules
			    for(var mod in jcf.modules) {
				    if(jcf.modules[mod] != jcf.modules[obj.name]) {
					    jcf.modules[mod].prototype.onModuleAdded(jcf.modules[obj.name]);
				    }
			    }
		    }
	    },
	    // add plugin to jcf.plugins
	    addPlugin: function(obj) {
		    if(obj && obj.name) {
			    jcf.plugins[obj.name] = function() {
				    this.init.apply(this, arguments);
			    }
			    for(var p in obj) {
				    jcf.plugins[obj.name].prototype[p] = obj[p];
			    }
		    }
	    },
	    // miscellaneous init
	    init: function(){
		    if(navigator.pointerEnabled) {
			    this.eventPress = 'pointerdown';
			    this.eventMove = 'pointermove';
			    this.eventRelease = 'pointerup';
		    } else if(navigator.msPointerEnabled) {
			    this.eventPress = 'MSPointerDown';
			    this.eventMove = 'MSPointerMove';
			    this.eventRelease = 'MSPointerUp';
		    } else {
			    this.eventPress = this.isTouchDevice ? 'touchstart' : 'mousedown';
			    this.eventMove = this.isTouchDevice ? 'touchmove' : 'mousemove';
			    this.eventRelease = this.isTouchDevice ? 'touchend' : 'mouseup';
		    }

		    setTimeout(function(){
			    jcf.lib.domReady(function(){
				    jcf.initStyles();
			    });
		    },1);
		    return this;
	    },
	    initStyles: function() {
		    // create <style> element and rules
		    var head = document.getElementsByTagName('head')[0],
			    style = document.createElement('style'),
			    rules = document.createTextNode('.'+jcf.baseOptions.unselectableClass+'{'+
				    '-moz-user-select:none;'+
				    '-webkit-tap-highlight-color:rgba(255,255,255,0);'+
				    '-webkit-user-select:none;'+
				    'user-select:none;'+
			    '}');

		    // append style element
		    style.type = 'text/css';
		    if(style.styleSheet) {
			    style.styleSheet.cssText = rules.nodeValue;
		    } else {
			    style.appendChild(rules);
		    }
		    head.appendChild(style);
	    }
    }.init();

    /*
     * Custom Form Control prototype
     */
    jcf.setBaseModule({
	    init: function(){
		    if(this.options.replaces) {
			    this.realElement = this.options.replaces;
			    this.realElement.jcf = this;
			    this.replaceObject();
		    }
	    },
	    defaultOptions: {
		    // default module options (will be merged with base options)
	    },
	    checkElement: function(el){
		    return true; // additional check for correct form element
	    },
	    replaceObject: function(){
		    this.createWrapper();
		    this.attachEvents();
		    this.fixStyles();
		    this.setupWrapper();
	    },
	    createWrapper: function(){
		    this.fakeElement = jcf.lib.createElement(this.options.wrapperTag);
		    this.labelFor = jcf.lib.getLabelFor(this.realElement);
		    jcf.lib.disableTextSelection(this.fakeElement);
		    jcf.lib.addClass(this.fakeElement, jcf.lib.getAllClasses(this.realElement.className, this.options.classPrefix));
		    jcf.lib.addClass(this.realElement, jcf.baseOptions.hiddenClass);
	    },
	    attachEvents: function(){
		    jcf.lib.event.add(this.realElement, 'focus', this.onFocusHandler, this);
		    jcf.lib.event.add(this.realElement, 'blur', this.onBlurHandler, this);
		    jcf.lib.event.add(this.fakeElement, 'click', this.onFakeClick, this);
		    jcf.lib.event.add(this.fakeElement, jcf.eventPress, this.onFakePressed, this);
		    jcf.lib.event.add(this.fakeElement, jcf.eventRelease, this.onFakeReleased, this);

		    if(this.labelFor) {
			    this.labelFor.jcf = this;
			    jcf.lib.event.add(this.labelFor, 'click', this.onFakeClick, this);
			    jcf.lib.event.add(this.labelFor, jcf.eventPress, this.onFakePressed, this);
			    jcf.lib.event.add(this.labelFor, jcf.eventRelease, this.onFakeReleased, this);
		    }
	    },
	    fixStyles: function() {
		    // hide mobile webkit tap effect
		    if(jcf.isTouchDevice) {
			    var tapStyle = 'rgba(255,255,255,0)';
			    this.realElement.style.webkitTapHighlightColor = tapStyle;
			    this.fakeElement.style.webkitTapHighlightColor = tapStyle;
			    if(this.labelFor) {
				    this.labelFor.style.webkitTapHighlightColor = tapStyle;
			    }
		    }
	    },
	    setupWrapper: function(){
		    // implement in subclass
	    },
	    refreshState: function(){
		    // implement in subclass
	    },
	    destroy: function() {
		    if(this.fakeElement && this.fakeElement.parentNode) {
			    this.fakeElement.parentNode.removeChild(this.fakeElement);
		    }
		    jcf.lib.removeClass(this.realElement, jcf.baseOptions.hiddenClass);
		    this.realElement.jcf = null;
	    },
	    onFocus: function(){
		    // emulated focus event
		    jcf.lib.addClass(this.fakeElement,this.options.focusClass);
	    },
	    onBlur: function(cb){
		    // emulated blur event
		    jcf.lib.removeClass(this.fakeElement,this.options.focusClass);
	    },
	    onFocusHandler: function() {
		    // handle focus loses
		    if(this.focused) return;
		    this.focused = true;

		    // handle touch devices also
		    if(jcf.isTouchDevice) {
			    if(jcf.focusedInstance && jcf.focusedInstance.realElement != this.realElement) {
				    jcf.focusedInstance.onBlur();
				    jcf.focusedInstance.realElement.blur();
			    }
			    jcf.focusedInstance = this;
		    }
		    this.onFocus.apply(this, arguments);
	    },
	    onBlurHandler: function() {
		    // handle focus loses
		    if(!this.pressedFlag) {
			    this.focused = false;
			    this.onBlur.apply(this, arguments);
		    }
	    },
	    onFakeClick: function(){
		    if(jcf.isTouchDevice) {
			    this.onFocus();
		    } else if(!this.realElement.disabled) {
			    this.realElement.focus();
		    }
	    },
	    onFakePressed: function(e){
		    this.pressedFlag = true;
	    },
	    onFakeReleased: function(){
		    this.pressedFlag = false;
	    },
	    onCreateModule: function(){
		    // implement in subclass
	    },
	    onModuleAdded: function(module) {
		    // implement in subclass
	    },
	    onControlReady: function() {
		    // implement in subclass
	    }
    });

    /*
     * JCF Utility Library
     */
    jcf.lib = {
	    bind: function(func, scope){
		    return function() {
			    return func.apply(scope, arguments);
		    };
	    },
	    browser: (function() {
		    var ua = navigator.userAgent.toLowerCase(), res = {},
		    match = /(webkit)[ \/]([\w.]+)/.exec(ua) || /(opera)(?:.*version)?[ \/]([\w.]+)/.exec(ua) ||
				    /(msie) ([\w.]+)/.exec(ua) || ua.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([\w.]+))?/.exec(ua) || [];
		    res[match[1]] = true;
		    res.version = match[2] || "0";
		    res.safariMac = ua.indexOf('mac') != -1 && ua.indexOf('safari') != -1;
		    return res;
	    })(),
	    getOffset: function (obj) {
		    if (obj.getBoundingClientRect && !jcf.isWinPhoneDevice) {
			    var scrollLeft = window.pageXOffset || document.documentElement.scrollLeft || document.body.scrollLeft;
			    var scrollTop = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop;
			    var clientLeft = document.documentElement.clientLeft || document.body.clientLeft || 0;
			    var clientTop = document.documentElement.clientTop || document.body.clientTop || 0;
			    return {
				    top:Math.round(obj.getBoundingClientRect().top + scrollTop - clientTop),
				    left:Math.round(obj.getBoundingClientRect().left + scrollLeft - clientLeft)
			    };
		    } else {
			    var posLeft = 0, posTop = 0;
			    while (obj.offsetParent) {posLeft += obj.offsetLeft; posTop += obj.offsetTop; obj = obj.offsetParent;}
			    return {top:posTop,left:posLeft};
		    }
	    },
	    getScrollTop: function() {
		    return window.pageYOffset || document.documentElement.scrollTop;
	    },
	    getScrollLeft: function() {
		    return window.pageXOffset || document.documentElement.scrollLeft;
	    },
	    getWindowWidth: function(){
		    return document.compatMode=='CSS1Compat' ? document.documentElement.clientWidth : document.body.clientWidth;
	    },
	    getWindowHeight: function(){
		    return document.compatMode=='CSS1Compat' ? document.documentElement.clientHeight : document.body.clientHeight;
	    },
	    getStyle: function(el, prop) {
		    if (document.defaultView && document.defaultView.getComputedStyle) {
			    return document.defaultView.getComputedStyle(el, null)[prop];
		    } else if (el.currentStyle) {
			    return el.currentStyle[prop];
		    } else {
			    return el.style[prop];
		    }
	    },
	    getParent: function(obj, selector) {
		    while(obj.parentNode && obj.parentNode != document.body) {
			    if(obj.parentNode.tagName.toLowerCase() == selector.toLowerCase()) {
				    return obj.parentNode;
			    }
			    obj = obj.parentNode;
		    }
		    return false;
	    },
	    isParent: function(parent, child) {
		    while(child.parentNode) {
			    if(child.parentNode === parent) {
				    return true;
			    }
			    child = child.parentNode;
		    }
		    return false;
	    },
	    getLabelFor: function(object) {
		    var parentLabel = jcf.lib.getParent(object,'label');
		    if(parentLabel) {
			    return parentLabel;
		    } else if(object.id) {
			    return jcf.lib.queryBySelector('label[for="' + object.id + '"]')[0];
		    }
	    },
	    disableTextSelection: function(el){
		    if (typeof el.onselectstart !== 'undefined') {
			    el.onselectstart = function() {return false;};
		    } else if(window.opera) {
			    el.setAttribute('unselectable', 'on');
		    } else {
			    jcf.lib.addClass(el, jcf.baseOptions.unselectableClass);
		    }
	    },
	    enableTextSelection: function(el) {
		    if (typeof el.onselectstart !== 'undefined') {
			    el.onselectstart = null;
		    } else if(window.opera) {
			    el.removeAttribute('unselectable');
		    } else {
			    jcf.lib.removeClass(el, jcf.baseOptions.unselectableClass);
		    }
	    },
	    queryBySelector: function(selector, scope){
		    if(typeof scope === 'string') {
			    var result = [];
			    var holders = this.getElementsBySelector(scope);
			    for (var i = 0, contextNodes; i < holders.length; i++) {
				    contextNodes = Array.prototype.slice.call(this.getElementsBySelector(selector, holders[i]));
				    result = result.concat(contextNodes);
			    }
			    return result;
		    } else {
			    return this.getElementsBySelector(selector, scope);
		    }
	    },
	    prevSibling: function(node) {
		    while(node = node.previousSibling) if(node.nodeType == 1) break;
		    return node;
	    },
	    nextSibling: function(node) {
		    while(node = node.nextSibling) if(node.nodeType == 1) break;
		    return node;
	    },
	    fireEvent: function(element,event) {
		    if(element.dispatchEvent){
			    var evt = document.createEvent('HTMLEvents');
			    evt.initEvent(event, true, true );
			    return !element.dispatchEvent(evt);
		    }else if(document.createEventObject){
			    var evt = document.createEventObject();
			    return element.fireEvent('on'+event,evt);
		    }
	    },
	    inherit: function(Child, Parent) {
		    var F = function() { }
		    F.prototype = Parent.prototype
		    Child.prototype = new F()
		    Child.prototype.constructor = Child
		    Child.superclass = Parent.prototype
	    },
	    extend: function(obj) {
		    for(var i = 1; i < arguments.length; i++) {
			    for(var p in arguments[i]) {
				    if(arguments[i].hasOwnProperty(p)) {
					    obj[p] = arguments[i][p];
				    }
			    }
		    }
		    return obj;
	    },
	    hasClass: function (obj,cname) {
		    return (obj.className ? obj.className.match(new RegExp('(\\s|^)'+cname+'(\\s|$)')) : false);
	    },
	    addClass: function (obj,cname) {
		    if (!this.hasClass(obj,cname)) obj.className += (!obj.className.length || obj.className.charAt(obj.className.length - 1) === ' ' ? '' : ' ') + cname;
	    },
	    removeClass: function (obj,cname) {
		    if (this.hasClass(obj,cname)) obj.className=obj.className.replace(new RegExp('(\\s|^)'+cname+'(\\s|$)'),' ').replace(/\s+$/, '');
	    },
	    toggleClass: function(obj, cname, condition) {
		    if(condition) this.addClass(obj, cname); else this.removeClass(obj, cname);
	    },
	    createElement: function(tagName, options) {
		    var el = document.createElement(tagName);
		    for(var p in options) {
			    if(options.hasOwnProperty(p)) {
				    switch (p) {
					    case 'class': el.className = options[p]; break;
					    case 'html': el.innerHTML = options[p]; break;
					    case 'style': this.setStyles(el, options[p]); break;
					    default: el.setAttribute(p, options[p]);
				    }
			    }
		    }
		    return el;
	    },
	    setStyles: function(el, styles) {
		    for(var p in styles) {
			    if(styles.hasOwnProperty(p)) {
				    switch (p) {
					    case 'float': el.style.cssFloat = styles[p]; break;
					    case 'opacity': el.style.filter = 'progid:DXImageTransform.Microsoft.Alpha(opacity='+styles[p]*100+')'; el.style.opacity = styles[p]; break;
					    default: el.style[p] = (typeof styles[p] === 'undefined' ? 0 : styles[p]) + (typeof styles[p] === 'number' ? 'px' : '');
				    }
			    }
		    }
		    return el;
	    },
	    getInnerWidth: function(el) {
		    return el.offsetWidth - (parseInt(this.getStyle(el,'paddingLeft')) || 0) - (parseInt(this.getStyle(el,'paddingRight')) || 0);
	    },
	    getInnerHeight: function(el) {
		    return el.offsetHeight - (parseInt(this.getStyle(el,'paddingTop')) || 0) - (parseInt(this.getStyle(el,'paddingBottom')) || 0);
	    },
	    getAllClasses: function(cname, prefix, skip) {
		    if(!skip) skip = '';
		    if(!prefix) prefix = '';
		    return cname ? cname.replace(new RegExp('(\\s|^)'+skip+'(\\s|$)'),' ').replace(/[\s]*([\S]+)+[\s]*/gi,prefix+"$1 ") : '';
	    },
	    getElementsBySelector: function(selector, scope) {
		    if(typeof document.querySelectorAll === 'function') {
			    return (scope || document).querySelectorAll(selector);
		    }
		    var selectors = selector.split(',');
		    var resultList = [];
		    for(var s = 0; s < selectors.length; s++) {
			    var currentContext = [scope || document];
			    var tokens = selectors[s].replace(/^\s+/,'').replace(/\s+$/,'').split(' ');
			    for (var i = 0; i < tokens.length; i++) {
				    token = tokens[i].replace(/^\s+/,'').replace(/\s+$/,'');
				    if (token.indexOf('#') > -1) {
					    var bits = token.split('#'), tagName = bits[0], id = bits[1];
					    var element = document.getElementById(id);
					    if (tagName && element.nodeName.toLowerCase() != tagName) {
						    return [];
					    }
					    currentContext = [element];
					    continue;
				    }
				    if (token.indexOf('.') > -1) {
					    var bits = token.split('.'), tagName = bits[0] || '*', className = bits[1], found = [], foundCount = 0;
					    for (var h = 0; h < currentContext.length; h++) {
						    var elements;
						    if (tagName == '*') {
							    elements = currentContext[h].getElementsByTagName('*');
						    } else {
							    elements = currentContext[h].getElementsByTagName(tagName);
						    }
						    for (var j = 0; j < elements.length; j++) {
							    found[foundCount++] = elements[j];
						    }
					    }
					    currentContext = [];
					    var currentContextIndex = 0;
					    for (var k = 0; k < found.length; k++) {
						    if (found[k].className && found[k].className.match(new RegExp('(\\s|^)'+className+'(\\s|$)'))) {
							    currentContext[currentContextIndex++] = found[k];
						    }
					    }
					    continue;
				    }
				    if (token.match(/^(\w*)\[(\w+)([=~\|\^\$\*]?)=?"?([^\]"]*)"?\]$/)) {
					    var tagName = RegExp.$1 || '*', attrName = RegExp.$2, attrOperator = RegExp.$3, attrValue = RegExp.$4;
					    if(attrName.toLowerCase() == 'for' && this.browser.msie && this.browser.version < 8) {
						    attrName = 'htmlFor';
					    }
					    var found = [], foundCount = 0;
					    for (var h = 0; h < currentContext.length; h++) {
						    var elements;
						    if (tagName == '*') {
							    elements = currentContext[h].getElementsByTagName('*');
						    } else {
							    elements = currentContext[h].getElementsByTagName(tagName);
						    }
						    for (var j = 0; elements[j]; j++) {
							    found[foundCount++] = elements[j];
						    }
					    }
					    currentContext = [];
					    var currentContextIndex = 0, checkFunction;
					    switch (attrOperator) {
						    case '=': checkFunction = function(e) { return (e.getAttribute(attrName) == attrValue) }; break;
						    case '~': checkFunction = function(e) { return (e.getAttribute(attrName).match(new RegExp('(\\s|^)'+attrValue+'(\\s|$)'))) }; break;
						    case '|': checkFunction = function(e) { return (e.getAttribute(attrName).match(new RegExp('^'+attrValue+'-?'))) }; break;
						    case '^': checkFunction = function(e) { return (e.getAttribute(attrName).indexOf(attrValue) == 0) }; break;
						    case '$': checkFunction = function(e) { return (e.getAttribute(attrName).lastIndexOf(attrValue) == e.getAttribute(attrName).length - attrValue.length) }; break;
						    case '*': checkFunction = function(e) { return (e.getAttribute(attrName).indexOf(attrValue) > -1) }; break;
						    default : checkFunction = function(e) { return e.getAttribute(attrName) };
					    }
					    currentContext = [];
					    var currentContextIndex = 0;
					    for (var k = 0; k < found.length; k++) {
						    if (checkFunction(found[k])) {
							    currentContext[currentContextIndex++] = found[k];
						    }
					    }
					    continue;
				    }
				    tagName = token;
				    var found = [], foundCount = 0;
				    for (var h = 0; h < currentContext.length; h++) {
					    var elements = currentContext[h].getElementsByTagName(tagName);
					    for (var j = 0; j < elements.length; j++) {
						    found[foundCount++] = elements[j];
					    }
				    }
				    currentContext = found;
			    }
			    resultList = [].concat(resultList,currentContext);
		    }
		    return resultList;
	    },
	    scrollSize: (function(){
		    var content, hold, sizeBefore, sizeAfter;
		    function buildSizer(){
			    if(hold) removeSizer();
			    content = document.createElement('div');
			    hold = document.createElement('div');
			    hold.style.cssText = 'position:absolute;overflow:hidden;width:100px;height:100px';
			    hold.appendChild(content);
			    document.body.appendChild(hold);
		    }
		    function removeSizer(){
			    document.body.removeChild(hold);
			    hold = null;
		    }
		    function calcSize(vertical) {
			    buildSizer();
			    content.style.cssText = 'height:'+(vertical ? '100%' : '200px');
			    sizeBefore = (vertical ? content.offsetHeight : content.offsetWidth);
			    hold.style.overflow = 'scroll'; content.innerHTML = 1;
			    sizeAfter = (vertical ? content.offsetHeight : content.offsetWidth);
			    if(vertical && hold.clientHeight) sizeAfter = hold.clientHeight;
			    removeSizer();
			    return sizeBefore - sizeAfter;
		    }
		    return {
			    getWidth:function(){
				    return calcSize(false);
			    },
			    getHeight:function(){
				    return calcSize(true)
			    }
		    }
	    }()),
	    domReady: function (handler){
		    var called = false
		    function ready() {
			    if (called) return;
			    called = true;
			    handler();
		    }
		    if (document.addEventListener) {
			    document.addEventListener("DOMContentLoaded", ready, false);
		    } else if (document.attachEvent) {
			    if (document.documentElement.doScroll && window == window.top) {
				    function tryScroll(){
					    if (called) return
					    if (!document.body) return
					    try {
						    document.documentElement.doScroll("left")
						    ready()
					    } catch(e) {
						    setTimeout(tryScroll, 0)
					    }
				    }
				    tryScroll()
			    }
			    document.attachEvent("onreadystatechange", function(){
				    if (document.readyState === "complete") {
					    ready()
				    }
			    })
		    }
		    if (window.addEventListener) window.addEventListener('load', ready, false)
		    else if (window.attachEvent) window.attachEvent('onload', ready)
	    },
	    event: (function(){
		    var guid = 0;
		    function fixEvent(e) {
			    e = e || window.event;
			    if (e.isFixed) {
				    return e;
			    }
			    e.isFixed = true; 
			    e.preventDefault = e.preventDefault || function(){this.returnValue = false}
			    e.stopPropagation = e.stopPropagation || function(){this.cancelBubble = true}
			    if (!e.target) {
				    e.target = e.srcElement
			    }
			    if (!e.relatedTarget && e.fromElement) {
				    e.relatedTarget = e.fromElement == e.target ? e.toElement : e.fromElement;
			    }
			    if (e.pageX == null && e.clientX != null) {
				    var html = document.documentElement, body = document.body;
				    e.pageX = e.clientX + (html && html.scrollLeft || body && body.scrollLeft || 0) - (html.clientLeft || 0);
				    e.pageY = e.clientY + (html && html.scrollTop || body && body.scrollTop || 0) - (html.clientTop || 0);
			    }
			    if (!e.which && e.button) {
				    e.which = e.button & 1 ? 1 : (e.button & 2 ? 3 : (e.button & 4 ? 2 : 0));
			    }
			    if(e.type === "DOMMouseScroll" || e.type === 'mousewheel') {
				    e.mWheelDelta = 0;
				    if (e.wheelDelta) {
					    e.mWheelDelta = e.wheelDelta/120;
				    } else if (e.detail) {
					    e.mWheelDelta = -e.detail/3;
				    }
			    }
			    return e;
		    }
		    function commonHandle(event, customScope) {
			    event = fixEvent(event);
			    var handlers = this.events[event.type];
			    for (var g in handlers) {
				    var handler = handlers[g];
				    var ret = handler.call(customScope || this, event);
				    if (ret === false) {
					    event.preventDefault()
					    event.stopPropagation()
				    }
			    }
		    }
		    var publicAPI = {
			    add: function(elem, type, handler, forcedScope) {
				    if (elem.setInterval && (elem != window && !elem.frameElement)) {
					    elem = window;
				    }
				    if (!handler.guid) {
					    handler.guid = ++guid;
				    }
				    if (!elem.events) {
					    elem.events = {};
					    elem.handle = function(event) {
						    return commonHandle.call(elem, event);
					    }
				    }
				    if (!elem.events[type]) {
					    elem.events[type] = {};
					    if (elem.addEventListener) elem.addEventListener(type, elem.handle, false);
					    else if (elem.attachEvent) elem.attachEvent("on" + type, elem.handle);
					    if(type === 'mousewheel') {
						    publicAPI.add(elem, 'DOMMouseScroll', handler, forcedScope);
					    }
				    }
				    var fakeHandler = jcf.lib.bind(handler, forcedScope);
				    fakeHandler.guid = handler.guid;
				    elem.events[type][handler.guid] = forcedScope ? fakeHandler : handler;
			    },
			    remove: function(elem, type, handler) {
				    var handlers = elem.events && elem.events[type];
				    if (!handlers) return;
				    delete handlers[handler.guid];
				    for(var any in handlers) return;
				    if (elem.removeEventListener) elem.removeEventListener(type, elem.handle, false);
				    else if (elem.detachEvent) elem.detachEvent("on" + type, elem.handle);
				    delete elem.events[type];
				    for (var any in elem.events) return;
				    try {
					    delete elem.handle;
					    delete elem.events;
				    } catch(e) {
					    if(elem.removeAttribute) {
						    elem.removeAttribute("handle");
						    elem.removeAttribute("events");
					    }
				    }
				    if(type === 'mousewheel') {
					    publicAPI.remove(elem, 'DOMMouseScroll', handler);
				    }
			    }
		    }
		    return publicAPI;
	    }())
    }

    // custom select module
    jcf.addModule({
	    name:'select',
	    selector:'select',
	    defaultOptions: {
		    useNativeDropOnMobileDevices: true,
		    hideDropOnScroll: true,
		    showNativeDrop: false,
		    handleDropPosition: true,
		    selectDropPosition: 'bottom', // or 'top'
		    wrapperClass:'select-area',
		    focusClass:'select-focus',
		    dropActiveClass:'select-active',
		    selectedClass:'item-selected',
		    currentSelectedClass:'current-selected',
		    disabledClass:'select-disabled',
		    valueSelector:'span.center', 
		    optGroupClass:'optgroup',
		    openerSelector:'a.select-opener',		
		    selectStructure:'<span class="left"></span><span class="center"></span><a class="select-opener"></a>',
		    wrapperTag: 'span',
		    classPrefix:'select-',
		    dropMaxHeight: 200,
		    dropFlippedClass: 'select-options-flipped',
		    dropHiddenClass:'options-hidden',
		    dropScrollableClass:'options-overflow',
		    dropClass:'select-options',
		    dropClassPrefix:'drop-',
		    dropStructure:'<div class="drop-holder"><div class="drop-list"></div></div>',
		    dropSelector:'div.drop-list'
	    },
	    checkElement: function(el){
		    return (!el.size && !el.multiple);
	    },
	    setupWrapper: function(){
		    jcf.lib.addClass(this.fakeElement, this.options.wrapperClass);
		    this.realElement.parentNode.insertBefore(this.fakeElement, this.realElement);
		    this.fakeElement.innerHTML = this.options.selectStructure;
		    this.fakeElement.style.width = (this.realElement.offsetWidth > 0 ? this.realElement.offsetWidth + 'px' : 'auto');

		    // show native drop if specified in options
		    if(this.options.useNativeDropOnMobileDevices && (jcf.isTouchDevice || jcf.isWinPhoneDevice)) {
			    this.options.showNativeDrop = true;
		    }
		    if(this.options.showNativeDrop) {
			    this.fakeElement.appendChild(this.realElement);
			    jcf.lib.removeClass(this.realElement, this.options.hiddenClass);
			    jcf.lib.setStyles(this.realElement, {
				    top:0,
				    left:0,
				    margin:0,
				    padding:0,
				    opacity:0,
				    border:'none',
				    position:'absolute',
				    width: jcf.lib.getInnerWidth(this.fakeElement) - 1,
				    height: jcf.lib.getInnerHeight(this.fakeElement) - 1
			    });
			    jcf.lib.event.add(this.realElement, jcf.eventPress, function(){
				    this.realElement.title = '';
			    }, this)
		    }
		
		    // create select body
		    this.opener = jcf.lib.queryBySelector(this.options.openerSelector, this.fakeElement)[0];
		    this.valueText = jcf.lib.queryBySelector(this.options.valueSelector, this.fakeElement)[0];
		    jcf.lib.disableTextSelection(this.valueText);
		    this.opener.jcf = this;

		    if(!this.options.showNativeDrop) {
			    this.createDropdown();
			    this.refreshState();
			    this.onControlReady(this);
			    this.hideDropdown(true);
		    } else {
			    this.refreshState();
		    }
		    this.addEvents();
	    },
	    addEvents: function(){
		    if(this.options.showNativeDrop) {
			    jcf.lib.event.add(this.realElement, 'click', this.onChange, this);
		    } else {
			    jcf.lib.event.add(this.fakeElement, 'click', this.toggleDropdown, this);
		    }
		    jcf.lib.event.add(this.realElement, 'change', this.onChange, this);
	    },
	    onFakeClick: function() {
		    // do nothing (drop toggles by toggleDropdown method)
	    },
	    onFocus: function(){
		    jcf.modules[this.name].superclass.onFocus.apply(this, arguments);
		    if(!this.options.showNativeDrop) {
			    // Mac Safari Fix
			    if(jcf.lib.browser.safariMac) {
				    this.realElement.setAttribute('size','2');
			    }
			    jcf.lib.event.add(this.realElement, 'keydown', this.onKeyDown, this);
			    if(jcf.activeControl && jcf.activeControl != this) {
				    jcf.activeControl.hideDropdown();
				    jcf.activeControl = this;
			    }
		    }
	    },
	    onBlur: function(){
		    if(!this.options.showNativeDrop) {
			    // Mac Safari Fix
			    if(jcf.lib.browser.safariMac) {
				    this.realElement.removeAttribute('size');
			    }
			    if(!this.isActiveDrop() || !this.isOverDrop()) {
				    jcf.modules[this.name].superclass.onBlur.apply(this);
				    if(jcf.activeControl === this) jcf.activeControl = null;
				    if(!jcf.isTouchDevice) {
					    this.hideDropdown();
				    }
			    }
			    jcf.lib.event.remove(this.realElement, 'keydown', this.onKeyDown);
		    } else {
			    jcf.modules[this.name].superclass.onBlur.apply(this);
		    }
	    },
	    onChange: function() {
		    this.refreshState();
	    },
	    onKeyDown: function(e){
		    this.dropOpened = true;
		    jcf.tmpFlag = true;
		    setTimeout(function(){jcf.tmpFlag = false},100);
		    var context = this;
		    context.keyboardFix = true;
		    setTimeout(function(){
			    context.refreshState();
		    },10);
		    if(e.keyCode == 13) {
			    context.toggleDropdown.apply(context);
			    return false;
		    }
	    },
	    onResizeWindow: function(e){
		    if(this.isActiveDrop()) {
			    this.hideDropdown();
		    }
	    },
	    onScrollWindow: function(e){
		    if(this.options.hideDropOnScroll) {
			    this.hideDropdown();
		    } else if(this.isActiveDrop()) {
			    this.positionDropdown();
		    }
	    },
	    onOptionClick: function(e){
		    var opener = e.target && e.target.tagName && e.target.tagName.toLowerCase() == 'li' ? e.target : jcf.lib.getParent(e.target, 'li');
		    if(opener) {
			    this.dropOpened = true;
			    this.realElement.selectedIndex = parseInt(opener.getAttribute('rel'));
			    if(jcf.isTouchDevice) {
				    this.onFocus();
			    } else {
				    this.realElement.focus();
			    }
			    this.refreshState();
			    this.hideDropdown();
			    jcf.lib.fireEvent(this.realElement, 'change');
		    }
		    return false;
	    },
	    onClickOutside: function(e){
		    if(jcf.tmpFlag) {
			    jcf.tmpFlag = false;
			    return;
		    }
		    if(!jcf.lib.isParent(this.fakeElement, e.target) && !jcf.lib.isParent(this.selectDrop, e.target)) {
			    this.hideDropdown();
		    }
	    },
	    onDropHover: function(e){
		    if(!this.keyboardFix) {
			    this.hoverFlag = true;
			    var opener = e.target && e.target.tagName && e.target.tagName.toLowerCase() == 'li' ? e.target : jcf.lib.getParent(e.target, 'li');
			    if(opener) {
				    this.realElement.selectedIndex = parseInt(opener.getAttribute('rel'));
				    this.refreshSelectedClass(parseInt(opener.getAttribute('rel')));
			    }
		    } else {
			    this.keyboardFix = false;
		    }
	    },
	    onDropLeave: function(){
		    this.hoverFlag = false;
	    },
	    isActiveDrop: function(){
		    return !jcf.lib.hasClass(this.selectDrop, this.options.dropHiddenClass);
	    },
	    isOverDrop: function(){
		    return this.hoverFlag;
	    },
	    createDropdown: function(){
		    // remove old dropdown if exists
		    if(this.selectDrop) {
			    this.selectDrop.parentNode.removeChild(this.selectDrop);
		    }

		    // create dropdown holder
		    this.selectDrop = document.createElement('div');
		    this.selectDrop.className = this.options.dropClass;
		    this.selectDrop.innerHTML = this.options.dropStructure;
		    jcf.lib.setStyles(this.selectDrop, {position:'absolute'});
		    this.selectList = jcf.lib.queryBySelector(this.options.dropSelector,this.selectDrop)[0];
		    jcf.lib.addClass(this.selectDrop, this.options.dropHiddenClass);
		    document.body.appendChild(this.selectDrop);
		    this.selectDrop.jcf = this;
		    jcf.lib.event.add(this.selectDrop, 'click', this.onOptionClick, this);
		    jcf.lib.event.add(this.selectDrop, 'mouseover', this.onDropHover, this);
		    jcf.lib.event.add(this.selectDrop, 'mouseout', this.onDropLeave, this);
		    this.buildDropdown();
	    },
	    buildDropdown: function() {
		    // build select options / optgroups
		    this.buildDropdownOptions();

		    // position and resize dropdown
		    this.positionDropdown();

		    // cut dropdown if height exceedes
		    this.buildDropdownScroll();
	    },
	    buildDropdownOptions: function() {
		    this.resStructure = '';
		    this.optNum = 0;
		    for(var i = 0; i < this.realElement.children.length; i++) {
			    this.resStructure += this.buildElement(this.realElement.children[i], i) +'\n';
		    }
		    this.selectList.innerHTML = this.resStructure;
	    },
	    buildDropdownScroll: function() {
		    jcf.lib.addClass(this.selectDrop, jcf.lib.getAllClasses(this.realElement.className, this.options.dropClassPrefix, jcf.baseOptions.hiddenClass));
		    if(this.options.dropMaxHeight) {
			    if(this.selectDrop.offsetHeight > this.options.dropMaxHeight) {
				    this.selectList.style.height = this.options.dropMaxHeight+'px';
				    this.selectList.style.overflow = 'auto';
				    this.selectList.style.overflowX = 'hidden';
				    jcf.lib.addClass(this.selectDrop, this.options.dropScrollableClass);
			    }
		    }
	    },
	    parseOptionTitle: function(optTitle) {
		    return (typeof optTitle === 'string' && /\.(jpg|gif|png|bmp|jpeg)(.*)?$/i.test(optTitle)) ? optTitle : '';
	    },
	    buildElement: function(obj, index){
		    // build option
		    var res = '', optImage;
		    if(obj.tagName.toLowerCase() == 'option') {
			    if(!jcf.lib.prevSibling(obj) || jcf.lib.prevSibling(obj).tagName.toLowerCase() != 'option') {
				    res += '<ul>';
			    }
			
			    optImage = this.parseOptionTitle(obj.title);
			    res += '<li rel="'+(this.optNum++)+'" class="'+(obj.className? obj.className + ' ' : '')+(index % 2 ? 'option-even ' : '')+'jcfcalc"><a href="#">'+(optImage ? '<img src="'+optImage+'" alt="" />' : '')+'<span>' + obj.innerHTML + '</span></a></li>';
			    if(!jcf.lib.nextSibling(obj) || jcf.lib.nextSibling(obj).tagName.toLowerCase() != 'option') {
				    res += '</ul>';
			    }
			    return res;
		    }
		    // build option group with options
		    else if(obj.tagName.toLowerCase() == 'optgroup' && obj.label) {
			    res += '<div class="'+this.options.optGroupClass+'">';
			    res += '<strong class="jcfcalc"><em>'+(obj.label)+'</em></strong>';
			    for(var i = 0; i < obj.children.length; i++) {
				    res += this.buildElement(obj.children[i], i);
			    }
			    res += '</div>';
			    return res;
		    }
	    },
	    positionDropdown: function(){
		    var ofs = jcf.lib.getOffset(this.fakeElement), selectAreaHeight = this.fakeElement.offsetHeight, selectDropHeight = this.selectDrop.offsetHeight;
		    var fitInTop = ofs.top - selectDropHeight >= jcf.lib.getScrollTop() && jcf.lib.getScrollTop() + jcf.lib.getWindowHeight() < ofs.top + selectAreaHeight + selectDropHeight;
		
		
		    if((this.options.handleDropPosition && fitInTop) || this.options.selectDropPosition === 'top') {
			    this.selectDrop.style.top = (ofs.top - selectDropHeight)+'px';
			    jcf.lib.addClass(this.selectDrop, this.options.dropFlippedClass);
			    jcf.lib.addClass(this.fakeElement, this.options.dropFlippedClass);
		    } else {
			    this.selectDrop.style.top = (ofs.top + selectAreaHeight)+'px';
			    jcf.lib.removeClass(this.selectDrop, this.options.dropFlippedClass);
			    jcf.lib.removeClass(this.fakeElement, this.options.dropFlippedClass);
		    }
		    this.selectDrop.style.left = ofs.left+'px';
		    this.selectDrop.style.width = this.fakeElement.offsetWidth+'px';
	    },
	    showDropdown: function(){
		    document.body.appendChild(this.selectDrop);
		    jcf.lib.removeClass(this.selectDrop, this.options.dropHiddenClass);
		    jcf.lib.addClass(this.fakeElement,this.options.dropActiveClass);
		    this.positionDropdown();

		    // highlight current active item
		    var activeItem = this.getFakeActiveOption();
		    this.removeClassFromItems(this.options.currentSelectedClass);
		    jcf.lib.addClass(activeItem, this.options.currentSelectedClass);
		
		    // show current dropdown
		    jcf.lib.event.add(window, 'resize', this.onResizeWindow, this);
		    jcf.lib.event.add(window, 'scroll', this.onScrollWindow, this);
		    jcf.lib.event.add(document, jcf.eventPress, this.onClickOutside, this);
		    this.positionDropdown();
	    },
	    hideDropdown: function(partial){
		    if(this.selectDrop.parentNode) {
			    if(this.selectDrop.offsetWidth) {
				    this.selectDrop.parentNode.removeChild(this.selectDrop);
			    }
			    if(partial) {
				    return;
			    }
		    }
		    if(typeof this.origSelectedIndex === 'number') {
			    this.realElement.selectedIndex = this.origSelectedIndex;
		    }
		    jcf.lib.removeClass(this.fakeElement,this.options.dropActiveClass);
		    jcf.lib.addClass(this.selectDrop, this.options.dropHiddenClass);
		    jcf.lib.event.remove(window, 'resize', this.onResizeWindow);
		    jcf.lib.event.remove(window, 'scroll', this.onScrollWindow);
		    jcf.lib.event.remove(document.documentElement, jcf.eventPress, this.onClickOutside);
		    if(jcf.isTouchDevice) {
			    this.onBlur();
		    }
	    },
	    toggleDropdown: function(){
		    if(!this.realElement.disabled) {
			    if(jcf.isTouchDevice) {
				    this.onFocus();
			    } else {
				    this.realElement.focus();
			    }
			    if(this.isActiveDrop()) {
				    this.hideDropdown();
			    } else {
				    this.showDropdown();
			    }
			    this.refreshState();
		    }
	    },
	    scrollToItem: function(){
		    if(this.isActiveDrop()) {
			    var dropHeight = this.selectList.offsetHeight;
			    var offsetTop = this.calcOptionOffset(this.getFakeActiveOption());
			    var sTop = this.selectList.scrollTop;
			    var oHeight = this.getFakeActiveOption().offsetHeight;
			    //offsetTop+=sTop;

			    if(offsetTop >= sTop + dropHeight) {
				    this.selectList.scrollTop = offsetTop - dropHeight + oHeight;
			    } else if(offsetTop < sTop) {
				    this.selectList.scrollTop = offsetTop;
			    }
		    }
	    },
	    getFakeActiveOption: function(c) {
		    return jcf.lib.queryBySelector('li[rel="'+(typeof c === 'number' ? c : this.realElement.selectedIndex) +'"]',this.selectList)[0];
	    },
	    calcOptionOffset: function(fake) {
		    var h = 0;
		    var els = jcf.lib.queryBySelector('.jcfcalc',this.selectList);
		    for(var i = 0; i < els.length; i++) {
			    if(els[i] == fake) break;
			    h+=els[i].offsetHeight;
		    }
		    return h;
	    },
	    childrenHasItem: function(hold,item) {
		    var items = hold.getElementsByTagName('*');
		    for(i = 0; i < items.length; i++) {
			    if(items[i] == item) return true;
		    }
		    return false;
	    },
	    removeClassFromItems: function(className){
		    var children = jcf.lib.queryBySelector('li',this.selectList);
		    for(var i = children.length - 1; i >= 0; i--) {
			    jcf.lib.removeClass(children[i], className);
		    }
	    },
	    setSelectedClass: function(c){
		    jcf.lib.addClass(this.getFakeActiveOption(c), this.options.selectedClass);
	    },
	    refreshSelectedClass: function(c){
		    if(!this.options.showNativeDrop) {
			    this.removeClassFromItems(this.options.selectedClass);
			    this.setSelectedClass(c);
		    }
		    if(this.realElement.disabled) {
			    jcf.lib.addClass(this.fakeElement, this.options.disabledClass);
			    if(this.labelFor) {
				    jcf.lib.addClass(this.labelFor, this.options.labelDisabledClass);
			    }
		    } else {
			    jcf.lib.removeClass(this.fakeElement, this.options.disabledClass);
			    if(this.labelFor) {
				    jcf.lib.removeClass(this.labelFor, this.options.labelDisabledClass);
			    }
		    }
	    },
	    refreshSelectedText: function() {
		    if(!this.dropOpened && this.realElement.title) {
			    this.valueText.innerHTML = this.realElement.title;
		    } else {
			    if(this.realElement.options[this.realElement.selectedIndex].title) {
				    var optImage = this.parseOptionTitle(this.realElement.options[this.realElement.selectedIndex].title);
				    this.valueText.innerHTML = (optImage ? '<img src="'+optImage+'" alt="" />' : '') + this.realElement.options[this.realElement.selectedIndex].innerHTML;
			    } else {
				    this.valueText.innerHTML = this.realElement.options[this.realElement.selectedIndex].innerHTML;
			    }
		    }
	    },
	    refreshState: function(){
		    this.origSelectedIndex = this.realElement.selectedIndex;
		    this.refreshSelectedClass();
		    this.refreshSelectedText();
		    if(!this.options.showNativeDrop) {
			    this.positionDropdown();
			    if(this.selectDrop.offsetWidth) {
				    this.scrollToItem();
			    }
		    }
	    }
    });

    // custom radio module
    jcf.addModule({
	    name:'radio',
	    selector: 'input[type="radio"]',
	    defaultOptions: {
		    wrapperClass:'rad-area',
		    focusClass:'rad-focus',
		    checkedClass:'rad-checked',
		    uncheckedClass:'rad-unchecked',
		    disabledClass:'rad-disabled',
		    radStructure:'<span></span>'
	    },
	    getRadioGroup: function(item){
		    var name = item.getAttribute('name');
		    if(name) {
			    return jcf.lib.queryBySelector('input[name="'+name+'"]', jcf.lib.getParent('form'));
		    } else {
			    return [item];
		    }
	    },
	    setupWrapper: function(){
		    jcf.lib.addClass(this.fakeElement, this.options.wrapperClass);
		    this.fakeElement.innerHTML = this.options.radStructure;
		    this.realElement.parentNode.insertBefore(this.fakeElement, this.realElement);
		    this.refreshState();
		    this.addEvents();
	    },
	    addEvents: function(){
		    jcf.lib.event.add(this.fakeElement, 'click', this.toggleRadio, this);
		    if(this.labelFor) {
			    jcf.lib.event.add(this.labelFor, 'click', this.toggleRadio, this);
		    }
	    },
	    onFocus: function(e) {
		    jcf.modules[this.name].superclass.onFocus.apply(this, arguments);
		    setTimeout(jcf.lib.bind(function(){
			    this.refreshState();
		    },this),10);
	    },
	    toggleRadio: function(){
		    if(!this.realElement.disabled && !this.realElement.checked) {
		        this.realElement.checked = true;
			    jcf.lib.fireEvent(this.realElement, 'change');
		    }
		    this.refreshState();
	    },
	    refreshState: function(){
		    var els = this.getRadioGroup(this.realElement);
		    for(var i = 0; i < els.length; i++) {
			    var curEl = els[i].jcf;
			    if(curEl) {
				    if(curEl.realElement.checked) {
					    jcf.lib.addClass(curEl.fakeElement, curEl.options.checkedClass);
					    jcf.lib.removeClass(curEl.fakeElement, curEl.options.uncheckedClass);
					    if(curEl.labelFor) {
						    jcf.lib.addClass(curEl.labelFor, curEl.options.labelActiveClass);
					    }
				    } else {
					    jcf.lib.removeClass(curEl.fakeElement, curEl.options.checkedClass);
					    jcf.lib.addClass(curEl.fakeElement, curEl.options.uncheckedClass);
					    if(curEl.labelFor) {
						    jcf.lib.removeClass(curEl.labelFor, curEl.options.labelActiveClass);
					    }
				    }
				    if(curEl.realElement.disabled) {
					    jcf.lib.addClass(curEl.fakeElement, curEl.options.disabledClass);
					    if(curEl.labelFor) {
						    jcf.lib.addClass(curEl.labelFor, curEl.options.labelDisabledClass);
					    }
				    } else {
					    jcf.lib.removeClass(curEl.fakeElement, curEl.options.disabledClass);
					    if(curEl.labelFor) {
						    jcf.lib.removeClass(curEl.labelFor, curEl.options.labelDisabledClass);
					    }
				    }
			    }
		    }
	    }
    });

    // custom checkbox module
    jcf.addModule({
	    name:'checkbox',
	    selector:'input[type="checkbox"]',
	    defaultOptions: {
		    wrapperClass:'chk-area',
		    focusClass:'chk-focus',
		    checkedClass:'chk-checked',
		    labelActiveClass:'chk-label-active',
		    uncheckedClass:'chk-unchecked',
		    disabledClass:'chk-disabled',
		    chkStructure:'<span></span>'
	    },
	    setupWrapper: function(){
		    jcf.lib.addClass(this.fakeElement, this.options.wrapperClass);
		    this.fakeElement.innerHTML = this.options.chkStructure;
		    this.realElement.parentNode.insertBefore(this.fakeElement, this.realElement);
		    jcf.lib.event.add(this.realElement, 'click', this.onRealClick, this);
		    this.refreshState();
	    },
	    isLinkTarget: function(target, limitParent) {
		    while(target.parentNode || target === limitParent) {
			    if(target.tagName.toLowerCase() === 'a') {
				    return true;
			    }
			    target = target.parentNode;
		    }
	    },
	    onFakePressed: function() {
		    jcf.modules[this.name].superclass.onFakePressed.apply(this, arguments);
		    if(!this.realElement.disabled) {
			    this.realElement.focus();
		    }
	    },
	    onFakeClick: function(e) {
		    jcf.modules[this.name].superclass.onFakeClick.apply(this, arguments);
		    this.tmpTimer = setTimeout(jcf.lib.bind(function(){
			    this.toggle();
		    },this),10);
		    if(!this.isLinkTarget(e.target, this.labelFor)) {
			    return false;
		    }
	    },
	    onRealClick: function(e) {
		    setTimeout(jcf.lib.bind(function(){
			    this.refreshState();
		    },this),10);
		    e.stopPropagation();
	    },
	    toggle: function(e){
		    if(!this.realElement.disabled) {
			    if(this.realElement.checked) {
			        this.realElement.checked = false;
			        this.realElement.removeAttribute("checked");
			    } else {
			        this.realElement.checked = true;
			        this.realElement.setAttribute("checked", "");
			    }
		    }
		    this.refreshState();
		    jcf.lib.fireEvent(this.realElement, 'change');
		    return false;
	    },
	    refreshState: function(){
		    if(this.realElement.checked) {
			    jcf.lib.addClass(this.fakeElement, this.options.checkedClass);
			    jcf.lib.removeClass(this.fakeElement, this.options.uncheckedClass);
			    if(this.labelFor) {
				    jcf.lib.addClass(this.labelFor, this.options.labelActiveClass);
			    }
		    } else {
			    jcf.lib.removeClass(this.fakeElement, this.options.checkedClass);
			    jcf.lib.addClass(this.fakeElement, this.options.uncheckedClass);
			    if(this.labelFor) {
				    jcf.lib.removeClass(this.labelFor, this.options.labelActiveClass);
			    }
		    }
		    if(this.realElement.disabled) {
			    jcf.lib.addClass(this.fakeElement, this.options.disabledClass);
			    if(this.labelFor) {
				    jcf.lib.addClass(this.labelFor, this.options.labelDisabledClass);
			    }
		    } else {
			    jcf.lib.removeClass(this.fakeElement, this.options.disabledClass);
			    if(this.labelFor) {
				    jcf.lib.removeClass(this.labelFor, this.options.labelDisabledClass);
			    }
		    }
	    }
    });

    // custom upload field module
    jcf.addModule({
	    name: 'file',
	    selector: 'input[type="file"]',
	    defaultOptions: {
		    buttonWidth: 30,
		    bigFontSize: 200,
		    buttonText:'Browse',
		    wrapperClass:'file-area',
		    focusClass:'file-focus',
		    disabledClass:'file-disabled',
		    opacityClass:'file-input-opacity',
		    noFileClass:'no-file',
		    extPrefixClass:'extension-',
		    uploadStructure:'<div class="jcf-input-wrapper"><div class="jcf-wrap"></div><label class="jcf-fake-input"><span><em></em></span></label><a class="jcf-upload-button"><span></span></a></div>',
		    uploadFileNameSelector:'label.jcf-fake-input span em',
		    uploadButtonSelector:'a.jcf-upload-button span',
		    inputWrapper: 'div.jcf-wrap'
	    },
	    setupWrapper: function(){
		    jcf.lib.addClass(this.fakeElement, this.options.wrapperClass);
		    this.fakeElement.innerHTML = this.options.uploadStructure;
		    this.realElement.parentNode.insertBefore(this.fakeElement, this.realElement);
		    this.fileNameInput = jcf.lib.queryBySelector(this.options.uploadFileNameSelector ,this.fakeElement)[0];
		    this.uploadButton = jcf.lib.queryBySelector(this.options.uploadButtonSelector ,this.fakeElement)[0];
		    this.inputWrapper = jcf.lib.queryBySelector(this.options.inputWrapper ,this.fakeElement)[0];

		    this.origElem = jcf.lib.nextSibling(this.realElement);
		    if(this.origElem && this.origElem.className.indexOf('file-input-text') > -1) {
			    this.origElem.parentNode.removeChild(this.origElem);
			    this.origTitle = this.origElem.innerHTML;
			    this.fileNameInput.innerHTML = this.origTitle;
		    }
		    this.uploadButton.innerHTML = this.realElement.title || this.options.buttonText;
		    this.realElement.removeAttribute('title');
		    this.fakeElement.style.position = 'relative';
		    this.realElement.style.position = 'absolute';
		    this.realElement.style.zIndex = 100;
		    this.inputWrapper.appendChild(this.realElement);
		    this.oTop = this.oLeft = this.oWidth = this.oHeight = 0;

		    jcf.lib.addClass(this.realElement, this.options.opacityClass);
		    jcf.lib.removeClass(this.realElement, jcf.baseOptions.hiddenClass);
		    this.inputWrapper.style.width = this.inputWrapper.parentNode.offsetWidth+'px';

		    this.shakeInput();
		    this.refreshState();
		    this.addEvents();
	    },
	    addEvents: function(){
		    jcf.lib.event.add(this.realElement, 'change', this.onChange, this);
		    if(!jcf.isTouchDevice) {
			    jcf.lib.event.add(this.fakeElement, 'mousemove', this.onMouseMove, this);
			    jcf.lib.event.add(this.fakeElement, 'mouseover', this.recalcDimensions, this);
		    }
	    },
	    onMouseMove: function(e){
		    this.realElement.style.top = Math.round(e.pageY - this.oTop - this.oHeight/2) + 'px';
		    this.realElement.style.left = (e.pageX - this.oLeft - this.oWidth + this.options.buttonWidth) + 'px';
	    },
	    onChange: function(){
		    this.refreshState();
	    },
	    getFileName: function(){
		    return this.realElement.value.replace(/^[\s\S]*(?:\\|\/)([\s\S^\\\/]*)$/g, "$1");
	    },
	    getFileExtension: function(){
		    return this.realElement.value.lastIndexOf('.') < 0 ? false : this.realElement.value.substring(this.realElement.value.lastIndexOf('.')+1).toLowerCase();
	    },
	    updateExtensionClass: function(){
		    var currentExtension = this.getFileExtension();
		    if(currentExtension) {
			    this.fakeElement.className = this.fakeElement.className.replace(new RegExp('(\\s|^)'+this.options.extPrefixClass+'[^ ]+','gi'),'')
			    jcf.lib.addClass(this.fakeElement, this.options.extPrefixClass+currentExtension)
		    }
	    },
	    shakeInput: function() {
		    // make input bigger
		    jcf.lib.setStyles(this.realElement, {
			    fontSize: this.options.bigFontSize,
			    lineHeight: this.options.bigFontSize,
			    heigth: 'auto',
			    top: 0,
			    left: this.inputWrapper.offsetWidth - this.realElement.offsetWidth
		    });
		    // IE styling fix
		    if((/(MSIE)/gi).test(navigator.userAgent)) {
			    this.tmpElement = document.createElement('span');
			    this.inputWrapper.insertBefore(this.tmpElement,this.realElement);
			    this.inputWrapper.insertBefore(this.realElement,this.tmpElement);
			    this.inputWrapper.removeChild(this.tmpElement);
		    }
	    },
	    recalcDimensions: function() {
		    var o = jcf.lib.getOffset(this.fakeElement);
		    this.oTop = o.top;
		    this.oLeft = o.left;
		    this.oWidth = this.realElement.offsetWidth;
		    this.oHeight = this.realElement.offsetHeight;
	    },
	    refreshState: function(){
		    jcf.lib.setStyles(this.realElement, {opacity: 0});
		    this.fileNameInput.innerHTML = this.getFileName() || this.origTitle || '';
		    if(this.realElement.disabled) {
			    jcf.lib.addClass(this.fakeElement, this.options.disabledClass);
			    if(this.labelFor) {
				    jcf.lib.addClass(this.labelFor, this.options.labelDisabledClass);
			    }
		    } else {
			    jcf.lib.removeClass(this.fakeElement, this.options.disabledClass);
			    if(this.labelFor) {
				    jcf.lib.removeClass(this.labelFor, this.options.labelDisabledClass);
			    }
		    }
		    if(this.realElement.value.length) {
			    jcf.lib.removeClass(this.fakeElement, this.options.noFileClass);
		    } else {
			    jcf.lib.addClass(this.fakeElement, this.options.noFileClass);
		    }
		    this.updateExtensionClass();
	    }
    });



    /*
     * FancyBox - jQuery Plugin
     * Simple and fancy lightbox alternative
     *
     * Examples and documentation at: http://fancybox.net
     * 
     * Copyright (c) 2008 - 2010 Janis Skarnelis
     * That said, it is hardly a one-person project. Many people have submitted bugs, code, and offered their advice freely. Their support is greatly appreciated.
     *
     * Version: 1.3.4 (11/11/2010)
     * Requires: jQuery v1.3+
     *
     * Dual licensed under the MIT and GPL licenses:
     *   http://www.opensource.org/licenses/mit-license.php
     *   http://www.gnu.org/licenses/gpl.html
     */
        ;(function(B){var L,T,Q,M,d,m,J,A,O,z,C=0,H={},j=[],e=0,G={},y=[],f=null,o=new Image(),i=/\.(jpg|gif|png|bmp|jpeg)(.*)?$/i,k=/[^\.]\.(swf)\s*$/i,p,N=1,h=0,t="",b,c,P=false,s=B.extend(B("<div/>")[0],{prop:0}),S=/MSIE 6/.test(navigator.userAgent)&&B.browser.version<7&&!window.XMLHttpRequest,r=function(){T.hide();o.onerror=o.onload=null;if(f){f.abort()}L.empty()},x=function(){if(false===H.onError(j,C,H)){T.hide();P=false;return}H.titleShow=false;H.width="auto";H.height="auto";L.html('<p id="fancybox-error">The requested content cannot be loaded.<br />Please try again later.</p>');n()},w=function(){var Z=j[C],W,Y,ab,aa,V,X;r();H=B.extend({},B.fn.fancybox.defaults,(typeof B(Z).data("fancybox")=="undefined"?H:B(Z).data("fancybox")));X=H.onStart(j,C,H);if(X===false){P=false;return}else{if(typeof X=="object"){H=B.extend(H,X)}}ab=H.title||(Z.nodeName?B(Z).attr("title"):Z.title)||"";if(Z.nodeName&&!H.orig){H.orig=B(Z).children("img:first").length?B(Z).children("img:first"):B(Z)}if(ab===""&&H.orig&&H.titleFromAlt){ab=H.orig.attr("alt")}W=H.href||(Z.nodeName?B(Z).attr("href"):Z.href)||null;if((/^(?:javascript)/i).test(W)||W=="#"){W=null}if(H.type){Y=H.type;if(!W){W=H.content}}else{if(H.content){Y="html"}else{if(W){if(W.match(i)){Y="image"}else{if(W.match(k)){Y="swf"}else{if(B(Z).hasClass("iframe")){Y="iframe"}else{if(W.indexOf("#")===0){Y="inline"}else{Y="ajax"}}}}}}}if(!Y){x();return}if(Y=="inline"){Z=W.substr(W.indexOf("#"));Y=B(Z).length>0?"inline":"ajax"}H.type=Y;H.href=W;H.title=ab;if(H.autoDimensions){if(H.type=="html"||H.type=="inline"||H.type=="ajax"){H.width="auto";H.height="auto"}else{H.autoDimensions=false}}if(H.modal){H.overlayShow=true;H.hideOnOverlayClick=false;H.hideOnContentClick=false;H.enableEscapeButton=false;H.showCloseButton=false}H.padding=parseInt(H.padding,10);H.margin=parseInt(H.margin,10);L.css("padding",(H.padding+H.margin));B(".fancybox-inline-tmp").unbind("fancybox-cancel").bind("fancybox-change",function(){B(this).replaceWith(m.children())});switch(Y){case"html":L.html(H.content);n();break;case"inline":if(B(Z).parent().is("#fancybox-content")===true){P=false;return}B('<div class="fancybox-inline-tmp" />').hide().insertBefore(B(Z)).bind("fancybox-cleanup",function(){B(this).replaceWith(m.children())}).bind("fancybox-cancel",function(){B(this).replaceWith(L.children())});B(Z).appendTo(L);n();break;case"image":P=false;B.fancybox.showActivity();o=new Image();o.onerror=function(){x()};o.onload=function(){P=true;o.onerror=o.onload=null;F()};o.src=W;break;case"swf":H.scrolling="no";aa='<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="'+H.width+'" height="'+H.height+'"><param name="movie" value="'+W+'"></param>';V="";B.each(H.swf,function(ac,ad){aa+='<param name="'+ac+'" value="'+ad+'"></param>';V+=" "+ac+'="'+ad+'"'});aa+='<embed src="'+W+'" type="application/x-shockwave-flash" width="'+H.width+'" height="'+H.height+'"'+V+"></embed></object>";L.html(aa);n();break;case"ajax":P=false;B.fancybox.showActivity();H.ajax.win=H.ajax.success;f=B.ajax(B.extend({},H.ajax,{url:W,data:H.ajax.data||{},dataType:"text",error:function(ac,ae,ad){if(ac.status>0){x()}},success:function(ad,af,ac){var ae=typeof ac=="object"?ac:f;if(ae.status==200||ae.status===0){if(typeof H.ajax.win=="function"){X=H.ajax.win(W,ad,af,ac);if(X===false){T.hide();return}else{if(typeof X=="string"||typeof X=="object"){ad=X}}}L.html(ad);n()}}}));break;case"iframe":E();break}},n=function(){var V=H.width,W=H.height;if(V.toString().indexOf("%")>-1){V=parseInt((B(window).width()-(H.margin*2))*parseFloat(V)/100,10)+"px"}else{V=V=="auto"?"auto":V+"px"}if(W.toString().indexOf("%")>-1){W=parseInt((B(window).height()-(H.margin*2))*parseFloat(W)/100,10)+"px"}else{W=W=="auto"?"auto":W+"px"}L.wrapInner('<div style="width:'+V+";height:"+W+";overflow: "+(H.scrolling=="auto"?"auto":(H.scrolling=="yes"?"scroll":"hidden"))+';position:relative;"></div>');H.width=L.width();H.height=L.height();E()},F=function(){H.width=o.width;H.height=o.height;B("<img />").attr({id:"fancybox-img",src:o.src,alt:H.title}).appendTo(L);E()},E=function(){var W,V;T.hide();if(M.is(":visible")&&false===G.onCleanup(y,e,G)){B('.fancybox-inline-tmp').trigger('fancybox-cancel');P=false;return}P=true;B(m.add(Q)).unbind();B(window).unbind("resize.fb scroll.fb");B(document).unbind("keydown.fb");if(M.is(":visible")&&G.titlePosition!=="outside"){M.css("height",M.height())}y=j;e=C;G=H;if(G.overlayShow){Q.css({"background-color":G.overlayColor,opacity:G.overlayOpacity,cursor:G.hideOnOverlayClick?"pointer":"auto",height:B(document).height()});if(!Q.is(":visible")){if(S){B("select:not(#fancybox-tmp select)").filter(function(){return this.style.visibility!=="hidden"}).css({visibility:"hidden"}).one("fancybox-cleanup",function(){this.style.visibility="inherit"})}Q.show()}}else{Q.hide()}c=R();l();if(M.is(":visible")){B(J.add(O).add(z)).hide();W=M.position(),b={top:W.top,left:W.left,width:M.width(),height:M.height()};V=(b.width==c.width&&b.height==c.height);m.fadeTo(G.changeFade,0.3,function(){var X=function(){m.html(L.contents()).fadeTo(G.changeFade,1,v)};B('.fancybox-inline-tmp').trigger('fancybox-change');m.empty().removeAttr("filter").css({"border-width":G.padding,width:c.width-G.padding*2,height:H.autoDimensions?"auto":c.height-h-G.padding*2});if(V){X()}else{s.prop=0;B(s).animate({prop:1},{duration:G.changeSpeed,easing:G.easingChange,step:U,complete:X})}});return}M.removeAttr("style");m.css("border-width",G.padding);if(G.transitionIn=="elastic"){b=I();m.html(L.contents());M.show();if(G.opacity){c.opacity=0}s.prop=0;B(s).animate({prop:1},{duration:G.speedIn,easing:G.easingIn,step:U,complete:v});return}if(G.titlePosition=="inside"&&h>0){A.show()}m.css({width:c.width-G.padding*2,height:H.autoDimensions?"auto":c.height-h-G.padding*2}).html(L.contents());M.css(c).fadeIn(G.transitionIn=="none"?0:G.speedIn,v)},D=function(V){if(V&&V.length){if(G.titlePosition=="float"){return'<table id="fancybox-title-float-wrap" cellpadding="0" cellspacing="0"><tr><td id="fancybox-title-float-left"></td><td id="fancybox-title-float-main">'+V+'</td><td id="fancybox-title-float-right"></td></tr></table>'}return'<div id="fancybox-title-'+G.titlePosition+'">'+V+"</div>"}return false},l=function(){t=G.title||"";h=0;A.empty().removeAttr("style").removeClass();if(G.titleShow===false){A.hide();return}t=B.isFunction(G.titleFormat)?G.titleFormat(t,y,e,G):D(t);if(!t||t===""){A.hide();return}A.addClass("fancybox-title-"+G.titlePosition).html(t).appendTo("body").show();switch(G.titlePosition){case"inside":A.css({width:c.width-(G.padding*2),marginLeft:G.padding,marginRight:G.padding});h=A.outerHeight(true);A.appendTo(d);c.height+=h;break;case"over":A.css({marginLeft:G.padding,width:c.width-(G.padding*2),bottom:G.padding}).appendTo(d);break;case"float":A.css("left",parseInt((A.width()-c.width-40)/2,10)*-1).appendTo(M);break;default:A.css({width:c.width-(G.padding*2),paddingLeft:G.padding,paddingRight:G.padding}).appendTo(M);break}A.hide()},g=function(){if(G.enableEscapeButton||G.enableKeyboardNav){B(document).bind("keydown.fb",function(V){if(V.keyCode==27&&G.enableEscapeButton){V.preventDefault();B.fancybox.close()}else{if((V.keyCode==37||V.keyCode==39)&&G.enableKeyboardNav&&V.target.tagName!=="INPUT"&&V.target.tagName!=="TEXTAREA"&&V.target.tagName!=="SELECT"){V.preventDefault();B.fancybox[V.keyCode==37?"prev":"next"]()}}})}if(!G.showNavArrows){O.hide();z.hide();return}if((G.cyclic&&y.length>1)||e!==0){O.show()}if((G.cyclic&&y.length>1)||e!=(y.length-1)){z.show()}},v=function(){if(B.support.opacity===false){m.get(0).style.removeAttribute("filter");M.get(0).style.removeAttribute("filter")}if(H.autoDimensions){m.css("height","auto")}M.css("height","auto");if(t&&t.length){A.show()}if(G.showCloseButton){J.show()}g();if(G.hideOnContentClick){m.bind("click",B.fancybox.close)}if(G.hideOnOverlayClick){Q.bind("click",B.fancybox.close)}B(window).bind("resize.fb",B.fancybox.resize);if(G.centerOnScroll){B(window).bind("scroll.fb",B.fancybox.center)}if(G.type=="iframe"){B('<iframe id="fancybox-frame" name="fancybox-frame'+new Date().getTime()+'" frameborder="0" hspace="0" '+(window.attachEvent?'allowtransparency="true""':"")+' scrolling="'+H.scrolling+'" src="'+G.href+'"></iframe>').appendTo(m)}M.show();P=false;B.fancybox.center();G.onComplete(y,e,G);K()},K=function(){var V,W;if((y.length-1)>e){V=y[e+1].href;if(typeof V!=="undefined"&&V.match(i)){W=new Image();W.src=V}}if(e>0){V=y[e-1].href;if(typeof V!=="undefined"&&V.match(i)){W=new Image();W.src=V}}},U=function(W){var V={width:parseInt(b.width+(c.width-b.width)*W,10),height:parseInt(b.height+(c.height-b.height)*W,10),top:parseInt(b.top+(c.top-b.top)*W,10),left:parseInt(b.left+(c.left-b.left)*W,10)};if(typeof c.opacity!=="undefined"){V.opacity=W<0.5?0.5:W}M.css(V);m.css({width:V.width-G.padding*2,height:V.height-(h*W)-G.padding*2})},u=function(){return[B(window).width()-(G.margin*2),B(window).height()-(G.margin*2),B(document).scrollLeft()+G.margin,B(document).scrollTop()+G.margin]},R=function(){var V=u(),Z={},W=G.autoScale,X=G.padding*2,Y;if(G.width.toString().indexOf("%")>-1){Z.width=parseInt((V[0]*parseFloat(G.width))/100,10)}else{Z.width=G.width+X}if(G.height.toString().indexOf("%")>-1){Z.height=parseInt((V[1]*parseFloat(G.height))/100,10)}else{Z.height=G.height+X}if(W&&(Z.width>V[0]||Z.height>V[1])){if(H.type=="image"||H.type=="swf"){Y=(G.width)/(G.height);if((Z.width)>V[0]){Z.width=V[0];Z.height=parseInt(((Z.width-X)/Y)+X,10)}if((Z.height)>V[1]){Z.height=V[1];Z.width=parseInt(((Z.height-X)*Y)+X,10)}}else{Z.width=Math.min(Z.width,V[0]);Z.height=Math.min(Z.height,V[1])}}Z.top=parseInt(Math.max(V[3]-20,V[3]+((V[1]-Z.height-40)*0.5)),10);Z.left=parseInt(Math.max(V[2]-20,V[2]+((V[0]-Z.width-40)*0.5)),10);return Z},q=function(V){var W=V.offset();W.top+=parseInt(V.css("paddingTop"),10)||0;W.left+=parseInt(V.css("paddingLeft"),10)||0;W.top+=parseInt(V.css("border-top-width"),10)||0;W.left+=parseInt(V.css("border-left-width"),10)||0;W.width=V.width();W.height=V.height();return W},I=function(){var Y=H.orig?B(H.orig):false,X={},W,V;if(Y&&Y.length){W=q(Y);X={width:W.width+(G.padding*2),height:W.height+(G.padding*2),top:W.top-G.padding-20,left:W.left-G.padding-20}}else{V=u();X={width:G.padding*2,height:G.padding*2,top:parseInt(V[3]+V[1]*0.5,10),left:parseInt(V[2]+V[0]*0.5,10)}}return X},a=function(){if(!T.is(":visible")){clearInterval(p);return}B("div",T).css("top",(N*-40)+"px");N=(N+1)%12};B.fn.fancybox=function(V){if(!B(this).length){return this}B(this).data("fancybox",B.extend({},V,(B.metadata?B(this).metadata():{}))).unbind("click.fb").bind("click.fb",function(X){X.preventDefault();if(P){return}P=true;B(this).blur();j=[];C=0;var W=B(this).attr("rel")||"";if(!W||W==""||W==="nofollow"){j.push(this)}else{j=B('a[rel="'+W+'"], area[rel="'+W+'"]');C=j.index(this)}w();return});return this};B.fancybox=function(Y){var X;if(P){return}P=true;X=typeof arguments[1]!=="undefined"?arguments[1]:{};j=[];C=parseInt(X.index,10)||0;if(B.isArray(Y)){for(var W=0,V=Y.length;W<V;W++){if(typeof Y[W]=="object"){B(Y[W]).data("fancybox",B.extend({},X,Y[W]))}else{Y[W]=B({}).data("fancybox",B.extend({content:Y[W]},X))}}j=jQuery.merge(j,Y)}else{if(typeof Y=="object"){B(Y).data("fancybox",B.extend({},X,Y))}else{Y=B({}).data("fancybox",B.extend({content:Y},X))}j.push(Y)}if(C>j.length||C<0){C=0}w()};B.fancybox.showActivity=function(){clearInterval(p);T.show();p=setInterval(a,66)};B.fancybox.hideActivity=function(){T.hide()};B.fancybox.next=function(){return B.fancybox.pos(e+1)};B.fancybox.prev=function(){return B.fancybox.pos(e-1)};B.fancybox.pos=function(V){if(P){return}V=parseInt(V);j=y;if(V>-1&&V<y.length){C=V;w()}else{if(G.cyclic&&y.length>1){C=V>=y.length?0:y.length-1;w()}}return};B.fancybox.cancel=function(){if(P){return}P=true;B('.fancybox-inline-tmp').trigger('fancybox-cancel');r();H.onCancel(j,C,H);P=false};B.fancybox.close=function(){if(P||M.is(":hidden")){return}P=true;if(G&&false===G.onCleanup(y,e,G)){P=false;return}r();B(J.add(O).add(z)).hide();B(m.add(Q)).unbind();B(window).unbind("resize.fb scroll.fb");B(document).unbind("keydown.fb");if(G.type==="iframe"){m.find("iframe").attr("src",S&&/^https/i.test(window.location.href||"")?"javascript:void(false)":"about:blank")}if(G.titlePosition!=="inside"){A.empty()}M.stop();function V(){Q.fadeOut("fast");A.empty().hide();M.hide();B('.fancybox-inline-tmp').trigger('fancybox-cleanup');m.empty();G.onClosed(y,e,G);y=H=[];e=C=0;G=H={};P=false}if(G.transitionOut=="elastic"){b=I();var W=M.position();c={top:W.top,left:W.left,width:M.width(),height:M.height()};if(G.opacity){c.opacity=1}A.empty().hide();s.prop=1;B(s).animate({prop:0},{duration:G.speedOut,easing:G.easingOut,step:U,complete:V})}else{M.fadeOut(G.transitionOut=="none"?0:G.speedOut,V)}};B.fancybox.resize=function(){if(Q.is(":visible")){Q.css("height",B(document).height())}B.fancybox.center(true)};B.fancybox.center=function(){var V,W;if(P){return}W=arguments[0]===true?1:0;V=u();if(!W&&(M.width()>V[0]||M.height()>V[1])){return}M.stop().animate({top:parseInt(Math.max(V[3]-20,V[3]+((V[1]-m.height()-40)*0.5)-G.padding)),left:parseInt(Math.max(V[2]-20,V[2]+((V[0]-m.width()-40)*0.5)-G.padding))},typeof arguments[0]=="number"?arguments[0]:200)};B.fancybox.init=function(){if(B("#fancybox-wrap").length){return}B("body").append(L=B('<div id="fancybox-tmp"></div>'),T=B('<div id="fancybox-loading"><div></div></div>'),Q=B('<div id="fancybox-overlay"></div>'),M=B('<div id="fancybox-wrap"></div>'));d=B('<div id="fancybox-outer"></div>').append('<div class="fancybox-bg" id="fancybox-bg-n"></div><div class="fancybox-bg" id="fancybox-bg-ne"></div><div class="fancybox-bg" id="fancybox-bg-e"></div><div class="fancybox-bg" id="fancybox-bg-se"></div><div class="fancybox-bg" id="fancybox-bg-s"></div><div class="fancybox-bg" id="fancybox-bg-sw"></div><div class="fancybox-bg" id="fancybox-bg-w"></div><div class="fancybox-bg" id="fancybox-bg-nw"></div>').appendTo(M);d.append(m=B('<div id="fancybox-content"></div>'),J=B('<a id="fancybox-close"></a>'),A=B('<div id="fancybox-title"></div>'),O=B('<a href="javascript:;" id="fancybox-left"><span class="fancy-ico" id="fancybox-left-ico"></span></a>'),z=B('<a href="javascript:;" id="fancybox-right"><span class="fancy-ico" id="fancybox-right-ico"></span></a>'));J.click(B.fancybox.close);T.click(B.fancybox.cancel);O.click(function(V){V.preventDefault();B.fancybox.prev()});z.click(function(V){V.preventDefault();B.fancybox.next()});if(B.fn.mousewheel){M.bind("mousewheel.fb",function(V,W){if(P){V.preventDefault()}else{if(B(V.target).get(0).clientHeight==0||B(V.target).get(0).scrollHeight===B(V.target).get(0).clientHeight){V.preventDefault();B.fancybox[W>0?"prev":"next"]()}}})}if(B.support.opacity===false){M.addClass("fancybox-ie")}if(S){T.addClass("fancybox-ie6");M.addClass("fancybox-ie6");B('<iframe id="fancybox-hide-sel-frame" src="'+(/^https/i.test(window.location.href||"")?"javascript:void(false)":"about:blank")+'" scrolling="no" border="0" frameborder="0" tabindex="-1"></iframe>').prependTo(d)}};B.fn.fancybox.defaults={padding:10,margin:40,opacity:false,modal:false,cyclic:false,scrolling:"auto",width:560,height:340,autoScale:true,autoDimensions:true,centerOnScroll:false,ajax:{},swf:{wmode:"transparent"},hideOnOverlayClick:true,hideOnContentClick:false,overlayShow:true,overlayOpacity:0.7,overlayColor:"#777",titleShow:true,titlePosition:"float",titleFormat:null,titleFromAlt:false,transitionIn:"fade",transitionOut:"fade",speedIn:300,speedOut:300,changeSpeed:300,changeFade:"fast",easingIn:"swing",easingOut:"swing",showCloseButton:true,showNavArrows:true,enableEscapeButton:true,enableKeyboardNav:true,onStart:function(){},onCancel:function(){},onComplete:function(){},onCleanup:function(){},onClosed:function(){},onError:function(){}};B(document).ready(function(){B.fancybox.init()})})(jQuery);


    // init
    jQuery.initJcf = function () {
        initTabs();
        jcf.customForms.replaceAll();
        initAccordion();
        initLightbox();
        initPopups();
        initValidation();
        initCustomHover();
        jQuery('input, textarea').placeholder();
        initEdit();
        initEditVideo();
    };
    jQuery.jcfModule = jcf;


    return jQuery;
});
