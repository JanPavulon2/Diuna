import React from 'react';
import SwitchComponent from './components/SwitchComponent';
import './App.css';
import styles from './components/SwitchComponent.module.css';

const App: React.FC = () => {
    return (
        <div className="App">
            <h1>Diuna Smart Switches</h1>

            <div className={styles['switch-container']}>
                <SwitchComponent tag="Switch1" name="Terrarium Light" description="Main light switch for the terrarium" />
                <SwitchComponent tag="Switch2" name="Room Light" description="Switch for the room's main lighting" />
                <SwitchComponent tag="Switch3" name="Top Shelf LED" description="Switch for the top shelf LED strip" />
                <SwitchComponent tag="Switch4" name="Bottom Shelf LED" description="Switch for the bottom shelf LED strip" />
            </div>
        </div>
    );
};

export default App;