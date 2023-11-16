//funcion para agregar al equipo

function addToTeam(pokemonName) {
    // Obtener el equipo del usuario desde el localStorage
    let userTeam = JSON.parse(localStorage.getItem('userTeam')) || [];

    // Verificar si el Pokémon ya está en el equipo
    if (!userTeam.includes(pokemonName)) {
        // Verificar si el equipo ya tiene 6 Pokémon
        if (userTeam.length < 6) {
            // Agregar el nuevo Pokémon al equipo
            alert("Pokemon: " + pokemonName + " ahora es parte de tu equipo!!!")

            userTeam.push(pokemonName);

            // Almacenar el equipo actualizado en el localStorage
            localStorage.setItem('userTeam', JSON.stringify(userTeam));

            // Actualizar la lista del equipo en la interfaz
            updateTeamList();

        } else {
            alert('El equipo ya tiene 6 Pokémon. Elimina uno antes de agregar otro.');
        }
    } else {
        alert('Este Pokémon ya está en tu equipo.');
    }
}

// Función para eliminar un Pokémon del equipo del usuario
function removeFromTeam(pokemonName) {
    let userTeam = JSON.parse(localStorage.getItem('userTeam')) || [];

    // Filtrar el Pokémon que se va a eliminar
    userTeam = userTeam.filter(pokemon => pokemon !== pokemonName);

    // Actualizar el equipo en el localStorage
    localStorage.setItem('userTeam', JSON.stringify(userTeam));

    // Actualizar la lista del equipo en la interfaz
    updateTeamList();
}

// Función para actualizar la lista del equipo en la interfaz
function updateTeamList() {
    let userTeam = JSON.parse(localStorage.getItem('userTeam')) || [];
    let userTeamList = document.getElementById('userTeamList');

    // Limpiar la lista actual
    userTeamList.innerHTML = '';

    // Agregar los Pokémon del equipo a la lista
    userTeam.forEach(pokemon => {
        let listItem = document.createElement('li');
        listItem.textContent = pokemon;
        listItem.className = 'list-group-item d-flex justify-content-between align-items-center';

        // Botón para eliminar el Pokémon del equipo
        let removeButton = document.createElement('button');
        removeButton.textContent = 'Eliminar';
        removeButton.className = 'btn btn-danger';
        removeButton.onclick = function () {
            removeFromTeam(pokemon);
        };

        // Agregar el botón de eliminar al elemento de la lista
        listItem.appendChild(removeButton);

        // Agregar el elemento a la lista
        userTeamList.appendChild(listItem);
    });
}

// Actualizar la lista del equipo en la interfaz al cargar la página
updateTeamList();


//fin de logica para agregar o liminar a el equipo pokemon



//filtrado de elementos
        $(document).ready(function () {
            var typingTimer;
        var doneTypingInterval = 500;  // 0.5 segundos
        var $searchInput = $('#searchInput');

        $searchInput.on('input', function () {
            clearTimeout(typingTimer);
        typingTimer = setTimeout(doneTyping, doneTypingInterval);
            });

        function doneTyping() {
                var searchTerm = $searchInput.val().toLowerCase();

        $('.pokemon-card').each(function () {
                    var pokemonName = $(this).find('.card-title').text().toLowerCase();

        if (pokemonName.includes(searchTerm)) {
            $(this).show();
                    } else {
            $(this).hide();
                    }
                });
            }
        });
 
