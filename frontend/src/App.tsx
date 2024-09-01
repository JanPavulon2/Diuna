import React, { useEffect } from 'react';
import * as signalR from '@microsoft/signalr';
import SwitchComponent from './components/SwitchComponent';
import VideoStream from './components/VideoStream';
import styles from './components/SwitchComponent.module.css';

const App: React.FC = () => {

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl('https://localhost:7191/switchhub')  // Assuming SignalR project runs on port 5001
            .withAutomaticReconnect()
            .build();

        connection.on('ReceiveMessage', (switchTag, isOn) => {
            console.log(`Switch ${switchTag} is now ${isOn ? 'On' : 'Off'}`);
            // Handle the received message, update state or UI accordingly
        });

        connection.start().then(() => {
            console.log('Connected to SignalR hub');
        }).catch(error => {
            console.error('Error connecting to SignalR hub', error);
        });


        return () => {
            connection.stop();
        };
    }, []);

    return (
        <div className="App">
            <h1>Diuna Smart Switches</h1>

            <div className={styles['switch-container']}>
                <SwitchComponent tag="Switch1" name="Terrarium Light" description="Main light switch for the terrarium" />
                <SwitchComponent tag="Switch2" name="Room Light" description="Switch for the room's main lighting" />
                <SwitchComponent tag="Switch3" name="Top Shelf LED" description="Switch for the top shelf LED strip" />
                <SwitchComponent tag="Switch4" name="Bottom Shelf LED" description="Switch for the bottom shelf LED strip" />
            </div>


            {/* <div className={styles['switch-container']}>
                <VideoStream></VideoStream>
            </div> */}
        </div>
    );
};

export default App;