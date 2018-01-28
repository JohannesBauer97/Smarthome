var net = require('net');
var client = new net.Socket();
var sockets = [];
var color = '#000000';

const settings = {
    Host: 'localhost',
    Port: 5555
}

client.connect(settings.Port,settings.Host,function(){
    console.log('Client connected!');
    client.setEncoding("ascii");
});

client.on('data',function(data){
    console.log('DATA:', data);
    if (data[0] == '#') {
        color = data;
    }else if(data[0] == 'I'){
        client.write(color + " Mock",'ascii');
    }
    
});

client.on('error',function(error){
    console.log('ERROR', error);
});

client.on('close',function(){
    console.log('Connection Closed.');
});