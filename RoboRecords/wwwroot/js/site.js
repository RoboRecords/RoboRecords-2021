// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function toggle(x){
    var element = $("#" + x);
    if (element.is(':hidden'))
    {
        element.show();
    }
    else
    {
        element.hide();
    }
}

function toggleDropDown(x){
    
    var attr = x.getAttribute("class");
    if (attr.includes(" show"))
    {
        
        x.setAttribute("class", attr.split(" show")[0])
    }
    else
    {
        x.setAttribute("class", attr + " show")
    }
}

$(".hide-at-start").hide();