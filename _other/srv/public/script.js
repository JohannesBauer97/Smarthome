$(function(){
    socket = io();

    $("#txt_cmd").keyup(function(e){
        if (e.keyCode == 13) {
            console.log($('#txt_cmd').val());
            socket.emit('cmd', $('#txt_cmd').val());
        }
    });
    
});

function update(color){
    //console.log(color.toHEXString());
    socket.emit('colorpicker',color.toHEXString());
}