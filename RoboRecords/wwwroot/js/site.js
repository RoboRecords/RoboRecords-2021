/*
 * site.js: Various JavaScript helpers used throughout the website
 * Copyright (C) 2022, Ors <Riku-S> and Refrag <Refragg>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

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