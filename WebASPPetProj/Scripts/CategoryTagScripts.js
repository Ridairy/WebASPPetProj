
    function Delete(type, id) {
        var url = 'CatTagChange/Delete/' + type + '/' + id;
        location.replace(url);
    }
function Create(type) {
    var url = 'CatTagChange/Create/' + type + '/' + $('#NewName').val() + '/' + $('#NewDescr').val();
        console.log('Create');
        location.replace(url);
    }
function Change(type, id) {
    var Name = "[id='Name " + id + "']";
    var Descr = "[id='Descr " + id + "']";
    var url = 'CatTagChange/Change/' + type + '/' + id + '/' + $(Name).val() + '/' + $(Descr).val();
        location.replace(url);
    }
 

