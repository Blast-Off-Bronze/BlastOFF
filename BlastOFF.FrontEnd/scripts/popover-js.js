/*

Bootstrap Popover Ref:
http://getbootstrap.com/javascript/#popovers

Favicons Ref:
http://getfavicon.appspot.com/
(https:// links are available)

Popover ref:
Note: the JS is written, so that you can have unlimited number of popovers.
http://wolfslittlestore.be/2013/12/easy-popovers-with-bootstrap/
with:
http://jsfiddle.net/7Kd44/
*/

$(document).ready(function() {

  $('.po-markup > .po-link').popover({
    trigger: 'hover',
    html: true,  // must have if HTML is contained in popover

    // get the title and conent
    title: function() {
      return $(this).parent().find('.po-title').html();
    },
    content: function() {
      return $(this).parent().find('.po-body').html();
    },

    container: 'body',
    placement: 'right'

  });

});