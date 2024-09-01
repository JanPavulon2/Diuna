import React from 'react';

const VideoStream = () => {
    return (
        <div>
            <h2>Live Stream from Raspberry Pi Camera</h2>
            <img 
                src="http://192.168.74.123:7123/stream.mjpg" 
                alt="Raspberry Pi Camera Stream"
                width="800"
                height="600"
                style={{ border: '1px solid #ddd', borderRadius: '8px' }}
            />
        </div>
    );
};

export default VideoStream;