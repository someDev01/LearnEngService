import { User2 } from 'lucide-react';
import styles from '../profile/profile.module.css';

function Profile({email}){
    return(
        <div className={styles.profile_block}>
            <User2 size={32} color='#f58244'/>
            <div className={styles.info}>
                <p className={styles.email}>{email}</p>
            </div>
        </div>
    )
}

export default Profile; 