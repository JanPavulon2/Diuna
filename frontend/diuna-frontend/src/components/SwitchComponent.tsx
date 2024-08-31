import React, { useState, useEffect } from 'react';
import styles from './SwitchComponent.module.css';

interface SwitchComponentProps {
    tag: string;
    name: string;
    description: string;
}

const SwitchComponent: React.FC<SwitchComponentProps> = ({ tag, name, description }) => {
    const [isOn, setIsOn] = useState<boolean>(false);

    // Fetch the initial state of the switch
    useEffect(() => {
        const fetchSwitchState = async () => {
            try {
                const response = await fetch(`https:localhost:7191/api/switch/${tag}`);
                const data = await response.json();
                setIsOn(data.isOn);
            } catch (error) {
                console.error('Error fetching switch state:', error);
            }
        };

        fetchSwitchState();
    }, [tag]);

    // Toggle the switch state
    const toggleSwitch = async () => {
        try {
            await fetch(`https://localhost:7191/api/switch/${tag}/toggle`, {
                method: 'POST',
            });
            setIsOn(prevIsOn => !prevIsOn);
        } catch (error) {
            console.error('Error toggling switch:', error);
        }
    };

    return (
        <div className={styles.switch}>
            <div className={styles.switchLed} style={{ backgroundColor: isOn ? 'green' : 'red' }}></div>
            <div className={styles.switchInfo}>
                <h3>{name}</h3>
                <p>{description}</p>
            </div>
            <button className={styles.button} onClick={toggleSwitch}>Toggle</button>
        </div>
    );
}

export default SwitchComponent;