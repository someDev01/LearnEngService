import { AlertCircle } from 'lucide-react';
import styles from '../vpn_warming/vpn_warming.module.css';

function VpnWarming({onClick}){
    return(
        <div className={styles.warming_block}>
            <div className={styles.warming}>
                <AlertCircle size={26} color='#e8b600'/>
                <p>Для просмотра youtube-видео вам понадобится vpn</p>
            </div>
            <button className={styles.close_warming_btn} onClick={onClick}>x</button>
        </div>
    )
}

export default VpnWarming;