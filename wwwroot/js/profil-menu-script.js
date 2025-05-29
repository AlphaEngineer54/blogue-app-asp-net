// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.getElementById('profileButton').addEventListener('click', function (event) {
    var menu = document.getElementById('profileMenu');
    menu.classList.toggle('show');
    event.stopPropagation();
});

document.addEventListener('click', function (event) {
    var menu = document.getElementById('profileMenu');
    if (menu.classList.contains('show')) {
        menu.classList.remove('show');
    }
});
