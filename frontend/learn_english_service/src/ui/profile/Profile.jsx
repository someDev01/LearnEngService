import { User2 } from 'lucide-react';
import styles from '../profile/profile.module.css';

function Profile({email}){
    return(
        <div className={styles.profile_block}>
            <div className={styles.profile_circle}>
                <p>{email[0].trim().toUpperCase()}</p>
            </div>
            <div className={styles.user_email}>
                <p>{email}</p>
            </div>
        </div>
    )
}

export default Profile; 