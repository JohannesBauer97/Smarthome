$(function(){
    socket = io("http://localhost");

    socket.on('getStripes',function(stripes){
        console.log('getStripes:', stripes);
    });

    socket.on('getStripeColor',function(SockName,SockColor){
        console.log('getStripeColor:',SockName,SockColor);
    });
    
});

function getStripes(){
    socket.emit('getStripes');
}

function getStripeColor(stripename){
    socket.emit('getStripeColor',stripename);
}

function setStripeColor(stripename, stripecolor){
    socket.emit('setStripeColor',stripename,stripecolor);
}