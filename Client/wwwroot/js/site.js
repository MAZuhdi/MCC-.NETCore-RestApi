// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//getElementById
let btn = document.getElementById("btnclick");
console.log(btn);
let p = document.getElementById("judul");

//addEventListener
btn.addEventListener("click", () => {
    if (p.style.backgroundColor == 'blue') {
        p.innerHTML = "Pokemons";
        p.style.backgroundColor = null;
    } else {
        p.innerHTML = "berubah";
        p.style.backgroundColor = 'blue';
    }
});

$.ajax({
    url: "https://pokeapi.co/api/v2/pokemon"
}).done((result) => {
    console.log(result.results);
    let text = "";
    $.each(result.results, (key, val) => {
        text += `
                <tr>
                    <th>${key+1}</th>
                    <td>${val.name.charAt(0).toUpperCase() + val.name.slice(1)}</td>
                    <td>
                       <button type="button" class="btn btn-primary" data-toggle="modal" onclick="getDetail(\'${val.url}\')" data-target="#pokeModal">Detail</button>
                    </td>
                </tr>
                `
    })
    $(".tabelPoke").html(text);
}).fail((error) => {
    console.log(error);
});

function getDetail(link) {
    $.ajax({
        url: link
    }).done((result) => {
        console.log(result);

        //nama pokemon
        document.getElementById("pname").innerHTML = result.name.charAt(0).toUpperCase() + result.name.slice(1);

        //img
        document.getElementById("av").src = result.sprites.other.dream_world.front_default;
        console.log(result.sprites.other.dream_world.front_default);

        //types
        let types = "";
        $.each(result.types, (key, val) => {
            console.log(val.type.name);
            let color = "";
            if (val.type.name == "grass") {
                color = "success";
            }
            else if (val.type.name == "poison") {
                color = "dark";
            }
            else if (val.type.name == "fire") {
                color = "danger";
            }
            else if (val.type.name == "flying") {
                color = "warning";
            }
            else if (val.type.name == "normal") {
                color = "secondary";
            }
            else {
                color = "primary";
            }
            types += `<span class="badge badge-${color} p-3 mr-2">${val.type.name}</span>`;
        })
        document.getElementById("poke-type").innerHTML = types;

        //stat
        let stats = "";
        $.each(result.stats, (key, val) => {
            console.log(val.stat.name + ": " + val.base_stat);
            stats += `
                        <tr>
                            <td class="font-weight-bold">${val.stat.name.toUpperCase()}</td>
                            <td class="font-weight-bold">${val.base_stat}</td>
                        </tr>
                     `
        })
        document.getElementById("statlist").innerHTML = stats;

        //height
        document.getElementById("poke-height").innerHTML = result.height + " m"
        //weight
        document.getElementById("poke-weight").innerHTML = result.weight + " Kg"

        //ability
        let abilities = "";
        $.each(result.abilities, (key, val) => {
            console.log(val.ability.name);
            abilities += `<span class="badge badge-light mr-2">${val.ability.name}</span>`;
        })
        document.getElementById("poke-ability").innerHTML = abilities;

        //moves
        let moves = "";
        for (var i = 0; i < 9; i++) {
            moves += `<span class="badge badge-light mr-2">${result.moves[i].move.name}</span>`;
        }
        console.log(moves);
        document.getElementById("poke-move").innerHTML = moves;
    }).fail((error) => {
        console.log(error);
    });
}