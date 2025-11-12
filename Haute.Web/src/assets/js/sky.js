// JavaScript Document
/*
  loader
  show elements
  teaser
  preload
  countdown SETUP
  niceScroll
    niceScroll.resize
  mobile detect
*/


$(function() {
    "use strict";
    $(window).on("load", function() {
        // loader
        $(".screen-loader").fadeOut("slow");
        // show elements
        setTimeout(function() {
            $("#preload").delay(250).fadeOut(1500);
            $("#intro-wrapper").delay(2000).css({
                display: "none"
            }).fadeIn(3000);
            $(".md_text").delay(2000).css({
                display: "none"
            }).fadeIn(3000);
            $("#keyboard-wrapper").delay(2000).css({
                display: "none"
            }).fadeIn(3000);
            $(".logo-wrapper").delay(2000).css({
                display: "none"
            }).fadeIn(3000);
        }, 0);
        // teaser
        var tid = setInterval(animateTeaser, 4000);
        var animCount = 0;
        function animateTeaser() {
            animCount++;
            if (animCount > 3) {
                animCount = 0;
                $(".teaser-text-animation.active").fadeTo(300, 0, function() {
                    $(".teaser-text-animation").removeClass("active");
                    $(".teaser-text-animation").removeClass("first");
                    $(".teaser-normal").css({
                        marginTop: "100px"
                    });
                    $(".teaser-highlight").css({
                        marginTop: "-100px"
                    });
                    $(".teaser-text-animation:first").addClass("active").fadeTo(300, 1, function() {
                        $(".teaser-normal, .teaser-highlight, .teaser-text-animation:first").each(function(wordCount) {
                            $(this).animate({
                                marginTop: 0
                            }, {
                                duration: 400,
                                queue: false
                            });
                        });
                    });
                });
            } else {
                var nextAnim = $(".teaser-text-animation").get(animCount);
                $(".teaser-text-animation.active").fadeTo(300, 0, function() {
                    $(".teaser-text-animation").removeClass("active");
                    $(".teaser-normal").css({
                        marginTop: "100px"
                    });
                    $(".teaser-highlight").css({
                        marginTop: "-100px"
                    });
                    $(nextAnim).addClass("active").fadeTo(300, 1, function() {
                        $(".teaser-normal, .teaser-highlight", nextAnim).each(function(wordCount) {
                            $(this).animate({
                                marginTop: 0
                            }, {
                                duration: 400,
                                queue: false
                            });
                        });
                    });
                });
            }
        }
    });
    // preload
    $("#preload").css({
        display: "table"
    });
    $(document).on("ready", function() {
        // niceScroll
        $("body").niceScroll({
            cursorcolor: "#fff",
            cursorwidth: "5px",
            cursorborder: "1px solid #fff",
            cursorborderradius: "0px",
            zindex: "9999",
            scrollspeed: "60",
            mousescrollstep: "40"
        });
    });
    // niceScroll.resize
    $("body").getNiceScroll().resize();
    // mobile detect
    var isMobile = {
        Android: function() {
            return navigator.userAgent.match(/Android/i);
        },
        BlackBerry: function() {
            return navigator.userAgent.match(/BlackBerry/i);
        },
        iOS: function() {
            return navigator.userAgent.match(/iPhone|iPad|iPod/i);
        },
        Opera: function() {
            return navigator.userAgent.match(/Opera Mini/i);
        },
        Windows: function() {
            return navigator.userAgent.match(/IEMobile/i);
        },
        any: function() {
            return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
        }
    };
});