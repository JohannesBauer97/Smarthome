var net = require('net');
var http = require('http').Server();
var io = require('socket.io')(http);
var LEDSockets = [];
var ClientSockets = [];

/**
 * TCP SERVER
 */

net.createServer(function (socket) {
    socket.name = socket.remoteAddress + ":" + socket.remotePort 
    LEDSockets.push(socket);
    
    console.log("LED Socket " + socket.name + " connected!");
    socket.setEncoding("ascii")

    socket.on('data', function (data) {
        var savedSock = LEDSockets.find(s => s.name === socket.name);
        savedSock.color = data;
        console.log(socket.name, data);
    });

    socket.on('close', function () {
        LEDSockets.splice(LEDSockets.indexOf(socket), 1);
        console.log('LED Socket ' + socket.name + ' disconnected!',LEDSockets);
    });

    socket.on('error', function (error) {
        console.log('LED Socket Error:',error);
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
            LEDSocketNames.push(LEDsock.name);
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
        LEDsock.write('I','ascii');
        socket.emit('getStripeColor',LEDsock.name,LEDsock.color);
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

        LEDsock.write(SockColor,'ascii');
    });

});


