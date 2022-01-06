// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let judul = document.getElementById("judul");
console.log(judul);

judul.addEventListener("click", () => {
    if (judul.style.backgroundColor == 'blue') {
        judul.innerHTML = "Ini Judul";
        judul.style.backgroundColor = null;
    } else {
        judul.innerHTML = "berubah";
        judul.style.backgroundColor = 'blue';
    }
})

//getElementsByClassName
let pakeClass = document.getElementsByClassName("list");
pakeClass[0].style.backgroundColor = 'blue';

//querySelector
let pakeQuery = document.querySelectorAll("section#b li.list");
console.log(pakeQuery);

//jQuery
let pakejQuery = $(".list").html("Ini diubah dengan jQuery");


//Tugas Pemahaman Array of Object
const animals = [
    { name: 'bimo', species: 'cat', kelas: { name: "mamalia" } },
    { name: 'budi', species: 'cat', kelas: { name: "mamalia" } },
    { name: 'nemo', species: 'snail', kelas: { name: "invertebrata" } },
    { name: 'dori', species: 'cat', kelas: { name: "mamalia" } },
    { name: 'simba', species: 'snail', kelas: { name: "invertebrata" } }
];
console.log(animals);

let onlyCat = [];
for (var i = 0; i < animals.length; i++) {
    if (animals[i].species == 'cat') {
        onlyCat.push(animals[i]);
    }
}
console.log(onlyCat);

for (var i = 0; i < animals.length; i++) {
    if (animals[i].species == 'snail') {
        animals[i].kelas.name = 'Non-Mamalia';
    }
}
console.log(animals);