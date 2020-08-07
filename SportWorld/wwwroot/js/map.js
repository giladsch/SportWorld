let map;

function getMap() {
    map = new Microsoft.Maps.Map('#store-locations-map', {
        zoom: 8,
        center: new Microsoft.Maps.Location(31.7148747, 34.9275617)
    });
    performSearch();
    $('#search-btn').click(performSearch);
    $('#searchBox').keypress((e) => {
        if (e.which == 13)
            performSearch();
    });
}

function performSearch() {
    var searchValue = $('#search-box')[0].value;
    $.ajax({
        url: `/About/GetStoresByName?name=${searchValue}`,
        type: 'GET',
    }).done(data => showStores(data))
      .fail(() => showErrorMsg('failed getting data'));
}

function showErrorMsg(msg) {
    const html = '<span class="error-msg">' + msg + '</span>';
    $('#results-panel').html(html);
}

function showStores(stores) {
    if (!stores || stores.length === 0) {
        $('#results-panel').html('<h1 class="fontStyle">No store found</h1>'); 
        return;
    }
       
    addStoresToMap(stores);

    window.navigator.geolocation.getCurrentPosition(userPosition => {
        const itemsPromises = stores.map(async (store, i) => {
            const temp = await $.ajax({ 
                url: `/About/GetTemprature?lon=${store.lontitude}&lat=${store.latitude}`, 
                type: 'GET' });
            return [
                `<table class="listItem"><tr>`,
                `<td><span onclick="zoomOnMap(${store.latitude}, ${store.lontitude})" class="title">${store.name}</span></td></tr>`,
                `<tr><td>Distance: ${distance(userPosition.coords.longitude, userPosition.coords.latitude, 
                    store.lontitude, store.latitude)} km</td></tr>`,
                `<tr><td>Temprature: ${temp} °C</tr>`,
                `<tr><td>Opening Hours: ${store.openingHour}-${store.closingHour}</tr>`,
                `<tr><td><a href="/Store/Edit/${store.id}">Edit</a> | <a href="/Store/Delete/${store.id}">Delete</a></td></tr>`,
                `</table>`,
            ].join('');
        });

        Promise.all(itemsPromises).then(items => {
            const resultsPanel = $('#results-panel');
            resultsPanel.html(items.join(''));
        });
    });
}

function clearMap() {
    map.entities.clear();   
    $('#results-panel').html(''); 
}

function addStoresToMap(stores) {
     stores.forEach(store => {
        const loc = new Microsoft.Maps.Location(store.latitude, store.lontitude);
        const pin = new Microsoft.Maps.Pushpin(loc);
        Microsoft.Maps.Events.addHandler(pin, 'click', () => window.location = `/Store/ById/${store.id}`);
        map.entities.push(pin);
    });
}

function zoomOnMap(lat, lon) {
    const loc = new Microsoft.Maps.Location(lat, lon);
    map.setView({ center: loc, zoom: 15 });
}

function toRad(num) {
    return num * Math.PI / 180;
}

function distance(lon1, lat1, lon2, lat2) {
  var R = 6371; // Radius of the earth in km
  var dLat = toRad(lat2-lat1);
  var dLon = toRad(lon2-lon1); 
  var a = Math.sin(dLat/2) * Math.sin(dLat/2) +
          Math.cos(toRad(lat1)) * Math.cos(toRad(lat2)) * 
          Math.sin(dLon/2) * Math.sin(dLon/2); 
  var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a)); 
  var d = R * c; // Distance in km
  return d.toFixed(2);
}