$(document).ready(function () {
    // Funkcionalnost za Klima (index.html)
    if ($('#klimeTableBody').length) {
        loadKlime();
        loadMarkeOptions();

        $('#klimaForm').on('submit', function (e) {
            e.preventDefault();
            const id = $('#klimaId').val();
            const klimaData = {
                model: $('#model').val(),
                cijena: parseFloat($('#cijena').val()),
                garancija: parseInt($('#garancija').val()),
                energetskiUcinkovita: $('#energetskiUcinkovita').is(':checked'),
                markaId: parseInt($('#marka').val())
            };

            if (id) {
                // Ažuriranje postojeće klime
                $.ajax({
                    url: `/api/Klima/${id}`,
                    type: 'PUT',
                    contentType: 'application/json',
                    data: JSON.stringify(klimaData),
                    success: function () {
                        location.reload();
                    },
                    error: function () {
                        alert('Došlo je do greške prilikom ažuriranja klime.');
                    }
                });
            } else {
                // Kreiranje nove klime
                $.ajax({
                    url: '/api/Klima',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(klimaData),
                    success: function () {
                        location.reload();
                    },
                    error: function () {
                        alert('Došlo je do greške prilikom kreiranja klime.');
                    }
                });
            }
        });

        window.deleteKlima = function (id) {
            if (confirm('Jeste li sigurni da želite obrisati ovu klimu?')) {
                $.ajax({
                    url: `/api/Klima/${id}`,
                    type: 'DELETE',
                    success: function () {
                        location.reload();
                    },
                    error: function () {
                        alert('Došlo je do greške prilikom brisanja klime.');
                    }
                });
            }
        }

        window.editKlima = function (klima) {
            $('#klimaId').val(klima.id);
            $('#model').val(klima.model);
            $('#cijena').val(klima.cijena);
            $('#garancija').val(klima.garancija);
            $('#energetskiUcinkovita').prop('checked', klima.energetskiUcinkovita);
            $('#marka').val(klima.markaId);
        }

        function loadKlime() {
            $.get('/api/Klima', function (data) {
                data.forEach(klima => {
                    $('#klimeTableBody').append(`
                        <tr>
                            <td>${klima.id}</td>
                            <td>${klima.model}</td>
                            <td>${klima.cijena.toFixed(2)}</td>
                            <td>${klima.garancija}</td>
                            <td>${klima.energetskiUcinkovita ? 'Da' : 'Ne'}</td>
                            <td>${klima.marka.name}</td>
                            <td>
                                <button class="edit-btn" onclick='editKlima(${JSON.stringify(klima)})'>Uredi</button>
                                <button class="delete-btn" onclick='deleteKlima(${klima.id})'>Obriši</button>
                            </td>
                        </tr>
                    `);
                });
            });
        }

        function loadMarkeOptions() {
            $.get('/api/Marka', function (data) {
                data.forEach(marka => {
                    $('#marka').append(`<option value="${marka.id}">${marka.name}</option>`);
                });
            });
        }
    }

    // Funkcionalnost za Marka (UpravljanjeMarkama.html)
    if ($('#markeTableBody').length) {
        loadMarke();

        $('#markaForm').on('submit', function (e) {
            e.preventDefault();
            const id = $('#markaId').val();
            const markaData = {
                id: id ? parseInt(id) : 0,
                name: $('#name').val()
            };

            if (id) {
                // Ažuriranje postojeće marke
                $.ajax({
                    url: `/api/Marka/${id}`,
                    type: 'PUT',
                    contentType: 'application/json',
                    data: JSON.stringify(markaData),
                    success: function () {
                        location.reload();
                    },
                    error: function () {
                        alert('Došlo je do greške prilikom ažuriranja marke.');
                    }
                });
            } else {
                // Kreiranje nove marke
                $.ajax({
                    url: '/api/Marka',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(markaData),
                    success: function () {
                        location.reload();
                    },
                    error: function () {
                        alert('Došlo je do greške prilikom kreiranja marke.');
                    }
                });
            }
        });

        window.deleteMarka = function (id) {
            if (confirm('Jeste li sigurni da želite obrisati ovu marku?')) {
                $.ajax({
                    url: `/api/Marka/${id}`,
                    type: 'DELETE',
                    success: function () {
                        location.reload();
                    },
                    error: function () {
                        alert('Došlo je do greške prilikom brisanja marke.');
                    }
                });
            }
        }

        window.editMarka = function (marka) {
            $('#markaId').val(marka.id);
            $('#name').val(marka.name);
        }

        function loadMarke() {
            $.get('/api/Marka', function (data) {
                data.forEach(marka => {
                    $('#markeTableBody').append(`
                        <tr>
                            <td>${marka.id}</td>
                            <td>${marka.name}</td>
                            <td>
                                <button class="edit-btn" onclick='editMarka(${JSON.stringify(marka)})'>Uredi</button>
                                <button class="delete-btn" onclick='deleteMarka(${marka.id})'>Obriši</button>
                            </td>
                        </tr>
                    `);
                });
            });
        }
    }
});
