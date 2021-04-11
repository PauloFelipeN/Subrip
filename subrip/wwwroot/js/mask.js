function mascaraData(campoData) {
    var data = campoData.value;
    if (data.length == 2) {
        data = data + ':';
        document.forms[0].Offset.value = data;
        return true;
    }
    if (data.length == 5) {
        data = data + ':';
        document.forms[0].Offset.value = data;
        return true;
    }
    if (data.length == 8) {
        data = data + ',';
        document.forms[0].Offset.value = data;
        return true;
    }
}