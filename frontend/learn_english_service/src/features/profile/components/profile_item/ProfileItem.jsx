import { formatActivityDate } from '../../../../utils/format_activity_date/formatActivityDate';
import AppIcon from '../app_icon/AppIcon';
import styles from '../profile_item/profile_item.module.css';

function ProfileItem({
    Icon,
    title,
    subtitle,
    date,
    color,
    type=null,
    onClick
}){
    return(
        <article className={`${styles.item} ${onClick ? styles.clickable : ''}`} onClick={onClick}>
            <div className={styles.left_part}>
                <AppIcon Icon={Icon} className='icon' color={color}/>
            </div>
            <div className={styles.right_part}>
                <div className={styles.info}>
                    <p className={`${styles.title} ${type === 'exit' ? styles.exit : ''}`}>{title}</p>
                    {subtitle && <p className={styles.subtitle}>{subtitle}</p>}
                </div>
                {date && <div className={styles.date}>
                    {formatActivityDate(date)}
                </div>}
            </div>
        </article>
    )
}

export default ProfileItem;