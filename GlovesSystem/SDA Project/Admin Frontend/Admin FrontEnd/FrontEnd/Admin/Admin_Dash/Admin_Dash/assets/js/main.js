let toggle=document.querySelector('.toggle');
let navigation=document.querySelector('.navigation');
let main=document.querySelector('.main');
let unorderedlist=document.querySelector('.ul');

toggle.onclick = function(){
    navigation.classList.toggle('active');
    main.classList.toggle('active');
    unorderedlist.classList.toggle('for-adjustment');
}