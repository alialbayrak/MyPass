function OnFailure(xhr) {
    var start = xhr.responseText.indexOf('<title>') + 7;
    var end = xhr.responseText.indexOf('</title>');
    alert(xhr.responseText.substring(start, end));
}

function OnSuccess() {
    location.reload();
}