import { LockKeyholeIcon } from "lucide-react";
import styles from '../email_notice/email_notice.module.css';


function EmailNotice(){
    return(
        <div className={styles.email_notice}><LockKeyholeIcon size={13} color='#a9a9a9'/> Почта используется только для входа</div>
    )
}

export default EmailNotice;