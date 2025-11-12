import React from 'react';
import { Card, Badge } from 'reactstrap';
import { NavLink } from 'react-router-dom';
import classnames from 'classnames';
import { ContextMenuTrigger } from 'react-contextmenu';
import { Colxx } from 'components/common/CustomBootstrap';

// onCheckItem
const ThumbListView = ({ product, isSelect, collect }) => {
  return (
    <Colxx xxs="12" key={product.ID} className="mb-3">
      <ContextMenuTrigger id="menu_id" data={product.ID} collect={collect}>
        <Card
          // onClick={(event) => onCheckItem(event, product.ID)}
          className={classnames('d-flex flex-row', {
            active: isSelect,
          })}
        >
          <NavLink to={`?p=${product.id}`} className="d-flex">
            <img
              alt={product.title}
              src={product.img}
              className="list-thumbnail responsive border-0 card-img-left"
            />
          </NavLink>
          <div className="pl-2 d-flex flex-grow-1 min-width-zero">
            <div className="card-body align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero align-items-lg-center">
              <NavLink to={`?p=${product.id}`} className="w-40 w-sm-100">
                <p className="list-item-heading mb-1 truncate">
                  {product.CanisterCode}
                </p>
              </NavLink>
              <p className="mb-1 text-muted text-small w-15 w-sm-100">
                {product.Color}
              </p>
              <p className="mb-1 text-muted text-small w-15 w-sm-100">
                {product.CreatedDate}
              </p>
              <div className="w-15 w-sm-100">
                <Badge color={product.IsActive} pill>
                  {product.IsActive}
                </Badge>
              </div>
            </div>
            {/* <div className="custom-control custom-checkbox pl-1 align-self-center pr-4">
              <CustomInput
                className="item-check mb-0"
                type="checkbox"
                id={`check_${product.ID}`}
                checked={isSelect}
                onChange={() => {}}
                label=""
              />
            </div> */}
          </div>
        </Card>
      </ContextMenuTrigger>
    </Colxx>
  );
};

/* React.memo detail : https://reactjs.org/docs/react-api.html#reactpurecomponent  */
export default React.memo(ThumbListView);
