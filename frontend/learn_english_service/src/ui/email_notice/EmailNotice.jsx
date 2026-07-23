import { LockKeyholeIcon } from "lucide-react";
import styles from '../email_notice/email_notice.module.css';

function EmailNotice(){
    return(
        <small><LockKeyholeIcon size={13} color='#a9a9a9'/> Почта используется только для входа</small>
    )
}

export default EmailNotice;