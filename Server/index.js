var net = require('net');
var http = require('http').Server();
var io = require('socket.io')(http, {origins: '*:*'});
var LEDSockets = [];

/**
 * TCP SERVER
 */

net.createServer(function (socket) {
    socket.name = socket.remoteAddress + ":" + socket.remotePort 
    socket.color = "";
    socket.hostname = "";
    socket.firstConnect = true;
    LEDSockets.push(socket);
    
    console.log("LED Socket " + socket.name + " connected!");
    socket.setEncoding("ascii")
    socket.write('I','ascii');

    socket.on('data', function (data) {
        var savedSock = LEDSockets.find(s => s.name === socket.name);
        if(savedSock){
            var color = '';
            var name = '';
            for(var i = 0; i < data.length; i++){
                if(i < 7){
                    color += data[i];
                }else if(i > 7){
                    name += data[i];
                }
            }
            savedSock.color = color;
            savedSock.hostname = name;
            if(socket.firstConnect){
                var LEDSocketNames = [];
                LEDSockets.forEach(LEDsock => {
                    LEDSocketNames.push({name:LEDsock.name, hostname:LEDsock.hostname, color:LEDsock.color});
                });
                io.emit('getStripes',LEDSocketNames);
                socket.firstConnect = false;
            }
        
        }else{
            console.log("Unknown Data",socket.name, data);
        }
       
    });

    socket.on('close', function () {
        LEDSockets.splice(LEDSockets.indexOf(socket), 1);
        console.log('LED Socket ' + socket.name + ' disconnected!',LEDSockets);
        var LEDSocketNames = [];
        LEDSockets.forEach(LEDsock => {
            LEDSocketNames.push({name:LEDsock.name, hostname:LEDsock.hostname, color:LEDsock.color});
        });
        io.emit('getStripes',LEDSocketNames);
    });

    socket.on('error', function (error) {
        console.log('LED Socket Error:',error);
        var LEDSocketNames = [];
        LEDSockets.forEach(LEDsock => {
            LEDSocketNames.push({name:LEDsock.name, hostname:LEDsock.hostname, color:LEDsock.color});
        });
        io.emit('getStripes',LEDSocketNames);
    });
}).listen(5555);

/**
 * SOCKET IO SERVER
 */

/**
 * Starting HTTP Server
 */
http.listen(80, function(){
    console.log('listening on *:80');
});

io.on('connection', function(socket){
    console.log('WebClient connected.');
    
    /**
     * Gets all connected LEDStripes
     */
    socket.on('getStripes',function(){
        var LEDSocketNames = [];
        LEDSockets.forEach(LEDsock => {
            LEDSocketNames.push({name:LEDsock.name, hostname:LEDsock.hostname, color:LEDsock.color});
        });
        socket.emit('getStripes',LEDSocketNames);
    });

    /**
     * Gets the color of a connected LEDStripe
     */
    socket.on('getStripeColor',function(stripeName){
        if(!stripeName){
            return;
        }
        var LEDsock = LEDSockets.find(s => s.name === stripeName);
        if(LEDsock){
            LEDsock.write('I','ascii');
            socket.emit('getStripeColor',LEDsock.name,LEDsock.color);
        }
        
    });

    /**
     * Sets the color of a connected LEDStripe
     */
    socket.on('setStripeColor',function(SockName, SockColor){
        if(!SockName ||!SockColor){
            return;
        }

        var LEDsock = LEDSockets.find(s => s.name === SockName);

        if(!LEDsock){
            return;
        }
        console.log(SockColor);
        LEDsock.write(SockColor,'ascii');
        //LEDsock.write('I','ascii');
    });

});


