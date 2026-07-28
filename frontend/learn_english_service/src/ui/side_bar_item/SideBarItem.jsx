import { NavLink } from "react-router-dom";
import styles from '../side_bar_item/side_bar_item.module.css';

function SideBarItem({to, title , Icon, onAction , onClick, iconSize=18}){

    if(onAction){
        return (
            <div onClick={onAction} className={styles.nav_btn}>
                <Icon size={iconSize} />
                {title}
            </div>
        )
    }

    return(
        <NavLink
            to={to}
            onClick={onClick}
            className={({isActive}) => (`${styles.nav_btn} ${isActive ? styles.active : ''}`)}
        >
            {({ isActive }) => (
                <>
                    <Icon
                        size={iconSize}
                        color={isActive ? "#ff5e00" : "#e2e2e2"}
                    />
                    {title}
                </>
            )}
        </NavLink>
    )
}

export default SideBarItem;